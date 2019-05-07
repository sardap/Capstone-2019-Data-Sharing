function showViewFor(card) {
    card.find('.dsp-view').show();
    card.find('.dsp-edit').hide();
}

function showEditFor(card) {
    card.find('.dsp-edit').show();
    card.find('.dsp-view').hide();
}

function appendCard(dsp) {
    if (!dsp) {
        dsp = {
            name: '',
            type: '',
            severity: '',
            additional: ''
        };
    }

    var count = MODEL_COUNT;
    var rowHtml =
        `
        <div class="card mb-3">
            <div class="card-header" id="headingOne">
                <h2 class="mb-0">
                    <button class="btn btn-link" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                        Data Sharing Policy 1
                    </button>
                    <input type="checkbox" data-toggle="toggle">
                </h2>
            </div>

            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample">

                <div class="card-body">
                    <div class="dsp-title form-group">
                        <label>Title</label>
                        <input type="text" class="form-control dsp-edit" />
                    </div>

                    <div class="dsp-excluded form-group">
                        <label>Select none or more to exclude your biometric data from buyers</label>
                        <select multiple class="form-control dsp-edit">
                            <option>Foo search</option>
                            <option>Bar search</option>
                        </select>
                    </div>

                    <div class="dsp-min-price form-group">
                        <label>Minimum price for your biometric data</label>
                        <input type="number" step="0.01" class="form-control dsp-edit" />
                    </div>

                    <div class="dsp-time-period form-group">
                        <label>Set a time range of your biometric data to share</label>
                        <div class="row">
                            <div class="col">
                                <input type="time" class="form-control dsp-edit dsp-start-time" />
                            </div>
                            <div class="col">
                                <input type="time" class="form-control dsp-edit dsp-start-time" />
                            </div>
                        </div>
                    </div>

                    <button type="button" class="btn btn-primary dsp-edit">Save</button>
                    <button type="button" class="btn btn-light dsp-edit">Cancel</button>
                </div>
            </div>
        </div>
        `;
    // IMPORTANT: Notce the id attribute with the _
    // This is to pass back the Input from here to the code behind Registration.cshtml
    // It's disgusting, I don't like it but it is the fastest way I can think of for now.
    $('#dsp-list').append(
        rowHtml
    );

    return $('#dsp-list').find('.dsp-card').last();
}

function appendAdditionCard() {
    $('#dsp-list').append(`
        <div class="card">
            <div class="card-body">
                <p class="card-text">
                    <i class="fas fa-plus ml-2"></i><span id="add-dsp">Click here to add</span> a new data sharing policy.
                </p>
            </div>
        </div>`
    );
}

function removeAdditionCard() {
    $('#dsp-list').find('#add-dsp').last().parents('.card').last().remove();
}

//Add event handler.
$('#dsp-list').on('click', '#add-dsp', function () {
    var isCardListEmpty = $('#dsp-list .card').length === 1;
    if (isCardListEmpty) {
        $('#dsp-list .card').first().remove();
    } else {
        removeAdditionCard();
    }

    var newCard = appendCard();
    showEditFor(newCard);
});

//Edit event handler.
$('body').on('click', '#dsp-list .fa-edit', function () {
    removeAdditionCard();

    var card = $(this).closest('.dsp-card');
    $('div.form-group', card).each(function () {
        if ($(this).find('.dsp-edit').length > 0) {
            showEditFor($(this));
        }
    });
    showEditFor(card);
});

//Update event handler.
$('body').on('click', '#dsp-list .btn-primary', function () {
    var card = $(this).closest('.dsp-card');

    $('div.form-group', card).each(function () {
        if ($(this).find('.dsp-edit').length > 0 && $(this).find('i').length === 0) {
            $(this).find('.dsp-view').html($(this).find('.dsp-edit').val());
            showViewFor($(this));
        }
    });

    showViewFor(card);

    if ($('#dsp-list').find('#add-dsp').length === 0) appendAdditionCard();
});

//Cancel event handler.
$('#dsp-list').on('click', '.btn-light', function () {
    var card = $(this).closest('.dsp-card');
    var isCardNew = true;

    $('div.form-group', card).each(function () {
        if ($(this).find('.dsp-edit').length > 0 && $(this).find('i').length === 0) {
            isCardNew = !$(this).find('.dsp-view').html();
            $(this).find('.dsp-edit').val($(this).find('.dsp-view').html());
            showViewFor($(this));
        }
    });

    if (isCardNew)
        card.remove();
    else
        showViewFor(card);

    appendAdditionCard();
});

//Delete event handler.
$('#dsp-list').on('click', '.fa-trash', function () {
    var card = $(this).closest('.dsp-card');
    var dspName = card.find('.dsp-name').first().find('label').first().html();

    if (confirm('Do you want to delete "' + dspName + '"?')) {
        card.remove();
    }
});
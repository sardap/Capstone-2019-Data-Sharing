var count = 0;

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

    count++;
    var rowHtml =
        `
        <div class="card dsp-card mb-3" id="dsp_${count}">
            <div class="card-header" id="heading_${count}">
                <h4 class="mb-0">
                    <button class="btn btn-link" type="button" data-toggle="collapse" data-target="#collapse_${count}" aria-expanded="true" aria-controls="collapse_${count}">
                        Data Sharing Policy ${count}
                    </button>
                    <div class="badge badge-pill badge-success float-right mt-1">Active</div>
                </h4>

            </div>

            <div id="collapse_${count}" class="collapse show" aria-labelledby="heading_${count}" data-parent="#dsp_${count}">

                <div class="card-body">
                    <div class="dsp-excluded form-group">
                        <label class="dsp-view">The following buyers will not be allowed to purchase your data</label>
                        <span class="dsp-view"></span>
                        <label class="dsp-edit">Select none or more to exclude your biometric data from buyers</label>
                        <select multiple class="form-control dsp-edit">
                            <option>Foo search</option>
                            <option>Bar search</option>
                        </select>
                    </div>

                    <div class="dsp-min-price form-group">
                        <label class="dsp-view">You will be paid at least $<span class="dsp-view"></span> for your biometric data</label>
                        <label class="dsp-edit">Minimum price for your biometric data</label>
                        <input type="number" step="0.01" class="form-control dsp-edit" />
                    </div>

                    <div class="dsp-time-period form-group">
                        <label class="dsp-view">Only data that are recorded between 
                                <span class="dsp-view dsp-start-time"></span> to <span class="dsp-view dsp-end-time"></span>
                        </label>

                        <label class="dsp-edit">Set a time range of your biometric data to share</label>
                        <div class="row">
                            <div class="col">
                                <input type="time" class="form-control dsp-edit dsp-start-time" />
                            </div>
                            <div class="col">
                                <input type="time" class="form-control dsp-edit dsp-end-time" />
                            </div>
                        </div>
                    </div>
                    
                    <div class="dsp-active form-group dsp-edit">
                        <label class="dsp-edit">
                            Toggle this data sharing policy's status
                            <small class="form-text text-muted">
                                By setting the data sharing policy status to active, 
                                the restrictions which you speicified here will be followed.
                            </small>
                        </label>
                        <button class="btn btn-block btn-danger dsp-edit" id="dsp-toggle-btn">Disable data sharing policy</button>
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
                    <i class="fas fa-plus mr-2"></i><span id="add-dsp">Click here to add</span> a new data sharing policy.
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
            var view = $(this).find('span.dsp-view');
            var formField = $(this).find('.form-control.dsp-edit');
            var userInput = formField.val();

            if (Array.isArray(userInput)) userInput = userInput.join(', ');

            if (formField.length > 1) {
                var formFields = formField.toArray();
                formFields.forEach(function (field) {
                    var timeView;
                    if (field.classList.contains('dsp-start-time')) {
                        timeView = view.find('span.dsp-start-time');
                    }

                    if (field.classList.contains('dsp-end-time')) {
                        timeView = view.find('span.dsp-end-time');
                    }

                    timeView.html(userInput);
                    showViewFor(view);
                });

                return;
            }

            view.html(userInput);
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

$('#dsp-list').on('click', '#dsp-toggle-btn', function () {
    var card = $(this).closest('.dsp-card');
    var btn = card.find('#dsp-toggle-btn').first();
    var badge = card.find('.badge').first();

    if (btn.hasClass('btn-danger') || btn.hasClass('btn-success')) {
        btn.toggleClass('btn-danger');
        btn.toggleClass('btn-success');
        badge.toggleClass('badge-danger');
        badge.toggleClass('badge-success');

        if (btn.hasClass('btn-danger')) {
            btn.text('Activate this data sharing policy');
            badge.text('Active');
        } else if (btn.hasClass('btn-success')) {
            btn.text('Disable this data sharing policy');
            badge.text('Disabled');
        }
    }
});
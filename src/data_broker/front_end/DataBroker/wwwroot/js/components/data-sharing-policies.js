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
                        <span class="dsp-view"></span>
                        <label class="dsp-edit">Select none or more to exclude your biometric data from buyers</label>
                        <select multiple class="form-control dsp-edit">
                            <option>Foo search</option>
                            <option>Bar search</option>
                            <option value="" selected>None</option>
                        </select>
                    </div>

                    <div class="dsp-min-price form-group">
                        <label class="dsp-view">You will be paid at least $<span class="dsp-view"></span> for your biometric data.</label>
                        <label class="dsp-edit">Minimum price for your biometric data</label>
                        <div class="input-group">
                            <div class="input-group-prepend dsp-edit">
                                <div class="input-group-text">&dollar;</div>
                            </div>
                            <input type="number" step="0.01" value="0" class="form-control dsp-edit" />
                        </div>
                    </div>

                    <div class="dsp-time-period form-group">
                        <span class="dsp-view"></span>

                        <label class="dsp-edit">Set a time range of your biometric data to share</label>
                        <div class="row">
                            <div class="col">
                                <input type="date" class="form-control dsp-edit dsp-start-date" />
                            </div>
                            <div class="col">
                                <input type="time" class="form-control dsp-edit dsp-start-time" />
                            </div>
                            <div class="col">
                                <input type="date" class="form-control dsp-edit dsp-end-date" />
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
                                the restrictions which you specified here will be enforced.
                            </small>
                        </label>
                        <button class="btn btn-block btn-danger dsp-edit" id="dsp-toggle-btn">Disable data sharing policy</button>
                    </div>

                    <button type="button" class="btn btn-primary dsp-edit" id="save-dsp"><i class="far fa-save"></i> Save</button>
                    <button type="button" class="btn btn-light dsp-edit" id="cancel-dsp">Cancel</button>
                    <button type="button" class="btn btn-info dsp-view" id="edit-dsp"><i class="fas fa-edit"></i> Edit</button>
                    <button type="button" class="btn btn-danger dsp-view" id="remove-dsp"><i class="far fa-trash-alt"></i> Remove</button>
                </div>
            </div>
        </div>
        `;
    // IMPORTANT: Notice the id attribute with the _
    // This is to pass back the Input from here to the code behind Registration.cshtml
    // It's disgusting, I don't like it but it is the fastest way I can think of for now.
    $('#dsp-list').append(rowHtml);

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

$('body').on('click', '#dsp-list #edit-dsp', function () {
    removeAdditionCard();

    var card = $(this).closest('.dsp-card');
    $('div.form-group', card).each(function () {
        if ($(this).find('.dsp-edit').length > 0) {
            showEditFor($(this));
        }
    });
    showEditFor(card);
});

function displayTimeRangeFields(formField, view) {
    var formFields = formField.toArray();
    var anyEmptyFieldValue = formFields.some(field => !field || !field.value);

    if (anyEmptyFieldValue)
        view.html('Your health data will not be limited by time range.');
    else {
        view.html(
            `Only data that are recorded between 
             <span class="dsp-start-date"></span> @ <span class="dsp-start-time"></span> to <span class="dsp-end-date"></span> @ <span class="dsp-end-time"></span>
             will be available for purchases.<br/>Data Broker will not record any health data outside this time range.`
        );
        formFields.forEach(function (field) {
            displayTimeFieldValue(field, view);
        });
    }

    showViewFor(view);
}

function displayTimeFieldValue(inputField, view) {
    let timeView, timeClass;
    let isDate = false;

    if (inputField.classList.contains('dsp-start-date')) {
        timeClass = 'dsp-start-date';
        isDate = true;
    }

    if (inputField.classList.contains('dsp-start-time')) {
        timeClass = 'dsp-start-time';
    }

    if (inputField.classList.contains('dsp-end-date')) {
        timeClass = 'dsp-end-date';
        isDate = true;
    }

    if (inputField.classList.contains('dsp-end-time')) {
        timeClass = 'dsp-end-time';
    }

    let userInput = inputField.value;
    let displayValue = isDate ? new Date(userInput).toLocaleDateString() : userInput;
    timeView = view.find('.' + timeClass);
    $(timeView).html(displayValue);
}

function createUnorderedListOfSelectedBuyers(userInput) {
    if (!userInput.length || userInput.some(v => !v))
        return 'Buyers from all categories can purchase your health data.';

    const $ul = $('<ul>', {class: 'ml-2'}).append(userInput.map(i => $("<li>").text(i)));
    return 'The following users will not be able to purchase your health data:' + $ul.prop('outerHTML');
}

//Update event handler.
$('body').on('click', '#dsp-list #save-dsp', function () {
    var card = $(this).closest('.dsp-card');

    $('div.form-group', card).each(function () {
        if ($(this).find('.dsp-edit').length > 0 && $(this).find('i').length === 0) {
            var view = $(this).find('span.dsp-view');
            var formField = $(this).find('.form-control.dsp-edit');
            var userInput = formField.val();
            var viewText = (userInput || '').toString();
            var isFieldBuyersSelector = Array.isArray(userInput);
            var isFieldDurationOfShareTime = formField.length > 1;

            if (isFieldBuyersSelector) 
                viewText = createUnorderedListOfSelectedBuyers(userInput);

            if (isFieldDurationOfShareTime) {
                displayTimeRangeFields(formField, view);
                return;
            }

            view.html(viewText);
            showViewFor($(this));
        }
    });

    showViewFor(card);

    if ($('#dsp-list').find('#add-dsp').length === 0) appendAdditionCard();
});

//Cancel event handler.
$('#dsp-list').on('click', '#cancel-dsp', function () {
    var card = $(this).closest('.dsp-card');
    var isCardNew = true;

    $('div.form-group', card).each(function () {
        if ($(this).find('.dsp-edit').length > 0 && $(this).find('i').length === 0) {
            if ($(this).find('.dsp-view').html())
                isCardNew = false;

            var formField = $(this).find('.dsp-edit');
            var fieldValueFromView = $(this).find('span.dsp-view').html();
            formField.val(fieldValueFromView);

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
$('#dsp-list').on('click', '#remove-dsp', function () {
    var card = $(this).closest('.dsp-card');

    if (card && confirm('Do you want to delete this policy?'))
        card.remove();
});

$('#dsp-list').on('click', '#dsp-toggle-btn', function () {
    var card = $(this).closest('.dsp-card');
    var btn = card.find('#dsp-toggle-btn').first();
    var badge = card.find('.badge').first();

    if (btn.hasClass('btn-danger') || btn.hasClass('btn-success')) {
        if (btn.hasClass('btn-danger')) {
            btn.text('Activate this data sharing policy');
            badge.text('Disabled');
        } else if (btn.hasClass('btn-success')) {
            btn.text('Disable this data sharing policy');
            badge.text('Active');
        }

        btn.toggleClass('btn-danger');
        btn.toggleClass('btn-success');
        badge.toggleClass('badge-danger');
        badge.toggleClass('badge-success');
    }
});
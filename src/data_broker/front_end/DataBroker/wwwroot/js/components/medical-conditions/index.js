function showViewFor(card) {
    card.find('.mc-view').show();
    card.find('.mc-edit').hide();
}

function showEditFor(card) {
    card.find('.mc-edit').show();
    card.find('.mc-view').hide();
}

function appendCard(mc) {
    if (!mc) {
        mc = {
            name: '',
            type: '',
            severity: '',
            additional: ''
        };
    }

    var count = MODEL_COUNT;
    var rowHtml =
        `
        <div class="card mc-card mb-3"> 
        <div class="card-body"> 
        <div class="mc-edit-options mc-view"> 
        <i class="fas fa-edit" data-toggle="tooltip" data-placement="top" title="Edit"></i> <i class="fas fa-trash" data-toggle="tooltip" data-placement="top" title="Remove"></i> 
        </div> 
        <p class="card-text"> 
        <div class="mc-name form-group"> 
        <label>Name</label> 
        <div class="mc-view"></div> 
        <input class="form-control mc-edit" type="text" id="Input_MedicalConditions_${MODEL_COUNT}__Name" name="Input.MedicalConditions[${MODEL_COUNT}].Name" placeholder="Name of your medical condition" /> 
        </div> 
        <div class="mc-type form-group"> 
        <label>Type</label> 
        <div class="mc-view"></div> 
        <input type="text" class="form-control mc-edit" id="Input_MedicalConditions_${MODEL_COUNT}__Type" name="Input.MedicalConditions[${MODEL_COUNT}].Type" placeholder="Type of your medical condition" /> 
        </div> 
        <div class="mc-severity form-group"> 
        <label>Severity</label> 
        <div class="mc-view"></div> 
        <input type="number" min="0" max="10" step="1" class="form-control mc-edit" placeholder="Severity (min. 0 and max. 10)" id="Input_MedicalConditions_${MODEL_COUNT}__Severity" name="Input.MedicalConditions[${MODEL_COUNT}].Severity" /> 
        </div> 
        <div class="mc-additional form-group"> 
        <label>Additional Information</label> 
        <div class="mc-view"></div> 
        <textarea class="form-control mc-edit" placeholder="Additional Information" id="Input_MedicalConditions_${MODEL_COUNT}__AdditionalInfo" name="Input.MedicalConditions[${MODEL_COUNT}].AdditionalInfo" /> 
        </div> 
        </p> 
        <button type ="button" class="btn btn-primary mc-edit">Save</button>  
        <button type ="button" class="btn btn-light mc-edit">Cancel</button> 
        </div> 
        </div>
        `;
    // IMPORTANT: Notce the id attribute with the _
    // This is to pass back the Input from here to the code behind Registration.cshtml
    // It's disgusting, I don't like it but it is the fastest way I can think of for now.
    $('#mc-list').append(
        rowHtml
    );

    return $('#mc-list').find('.mc-card').last();
}

function appendAdditionCard() {
    $('#mc-list').append(
        '<div class="card">' +
        '<div class="card-body">' +
        '<p class="card-text">' +
        '<i class="fas fa-plus"></i> <span id="add-mc">Click here to add</span> medical conditions.' +
        '</p>' +
        '</div>' +
        '</div>'
    );
}

function removeAdditionCard() {
    $('#mc-list').find('#add-mc').last().parents('.card').last().remove();
}

//Add event handler.
$('#mc-list').on('click', '#add-mc', function () {
    var isCardListEmpty = $('#mc-list .card').length === 1;
    if (isCardListEmpty) {
        $('#mc-list .card').first().remove();
    } else {
        removeAdditionCard();
    }

    var newCard = appendCard();
    showEditFor(newCard);
});

//Edit event handler.
$('body').on('click', '#mc-list .fa-edit', function () {
    removeAdditionCard();

    var card = $(this).closest('.mc-card');
    $('div.form-group', card).each(function () {
        if ($(this).find('.mc-edit').length > 0) {
            showEditFor($(this));
        }
    });
    showEditFor(card);
});

//Update event handler.
$('body').on('click', '#mc-list .btn-primary', function () {
    var card = $(this).closest('.mc-card');

    $('div.form-group', card).each(function () {
        if ($(this).find('.mc-edit').length > 0 && $(this).find('i').length === 0) {
            $(this).find('.mc-view').html($(this).find('.mc-edit').val());
            showViewFor($(this));
        }
    });

    showViewFor(card);

    if ($('#mc-list').find('#add-mc').length === 0) appendAdditionCard();
});

//Cancel event handler.
$('#mc-list').on('click', '.btn-light', function () {
    var card = $(this).closest('.mc-card');
    var isCardNew = true;

    $('div.form-group', card).each(function () {
        if ($(this).find('.mc-edit').length > 0 && $(this).find('i').length === 0) {
            isCardNew = !$(this).find('.mc-view').html();
            $(this).find('.mc-edit').val($(this).find('.mc-view').html());
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
$('#mc-list').on('click', '.fa-trash', function () {
    var card = $(this).closest('.mc-card');
    var mcName = card.find('.mc-name').first().find('label').first().html();

    if (confirm('Do you want to delete "' + mcName + '"?')) {
        card.remove();
    }
});

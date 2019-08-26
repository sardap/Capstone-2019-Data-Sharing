import {makeMcListReadonly} from './medical-conditions.js';

var count = MODEL_COUNT;

function getRequest() {
    return `
    <div class="card request-item mb-3" id="request_${count}">
      <div class="card-header" id="request_${count}_header">
        ${getRequestHeader()}
      </div>
      <div
        id="collapse_request_${count}"
        class="collapse show"
        aria-labelledby="request_${count}_header"
        data-parent="#request_${count}"
      >
      <div class="card-body">
        ${getDescriptionBox()}
        ${getDataTypeField()}
        ${getGenderField()} 
        ${getAgeRangeField()}
        ${getEthnicityField()}
        ${getCountryField()}
        ${getAddMcButton()}
        ${getButtons()}
      </div>
      </div>
    </div>
    `;
}

function getRequestHeader() {
    return `
    <h4 class="mb-0">
      <div class="badge badge-pill badge-warning mt-1">Draft</div>
      <button
        class="btn"
        type="button"
        data-toggle="collapse"
        data-target="#collapse_request_${count}"
        aria-expanded="true"
        aria-controls="collapse_request_${count}"
      >
      <h5>Purchase Request #${count}</h5>
      </button>
    </h4>`;
}

function getDescriptionBox() {
    return `
    <div class="alert alert-warning request-description" role="alert">
      Please enter a list of criteria for the data subjects you would like to purchase from. <br/>
      You cannot alter these criteria once you have submitted your purchase request.
    </div>
    `;
}

function getDataTypeField() {
    return `
    <div class="form-group request-datatype">
      <span class="request-view"></span>
      <label for="request-datatype" class="request-edit"><b>Select one or more health data type</b></label>
      <select
        name="request-datatype"
        class="form-control request-edit"
        multiple="multiple"
      >
        <option value="heartrate">Heart Rate</option>
        <option value="height">Height</option>
        <option value="foo">Foo</option>
        <option value="bar">Bar</option>
      </select>
    </div>
    `;
}

function getGenderField() {
    return `
    <div class="form-group request-gender">
      <b>Select one or more genders</b>
      <span class="request-view"></span>
      <div class="request-edit">
        <div class="ml-2">
          <input
            type="checkbox"
            class="request-edit"
            name="request-gender-female"
            value="female"
          />
          <label for="request-gender-female">Female</label>
          <br />
          <input
            type="checkbox"
            class="request-edit"
            name="request-gender-male"
            value="male"
          />
          <label for="request-gender-male">Male</label>
          <br />
          <input
            type="checkbox"
            class="request-edit"
            name="request-gender-non-bin"
            value="Others"
          />
          <label for="request-gender-non-bin">Non-binary</label>
        </div>
      </div>
    </div>
    `;
}

function getAgeRangeField() {
    const DEFAULT_FROM = 18;
    const DEFAULT_TO = 24;

    return `
    <div class="form-group request-age">
      <b>Age range</b>
      <span class="request-view"></span>
      <p class="request-edit">
        <div class="row">
          <div class="col">
            <label for="request-age-from" class="request-edit">From</label>
            <input
              type="number"
              class="form-control request-edit"
              name="request-age-from"
              value="${DEFAULT_FROM}"
            />
          </div>
          <div class="col">
            <label for="request-ade-to" class="request-edit">To</label>
            <input
              type="number"
              class="form-control request-edit"
              name="request-age-to"
              value="${DEFAULT_TO}"
            />
          </div>
        </div>
      </p>
    </div>
    `;
}

function getEthnicityField() {
    return `
    <div class="form-group request-ethnicity">
      <span class="request-view"></span>
      <p class="request-edit">
        <b>Ethnicity</b>
        <fieldset>
          <select class="form-control dropdown" id="ethnicity" name="ethnicity">
            <option value="" selected="selected">Any</option>
            <optgroup label="White">
              <option value="White English">English</option>
              <option value="White Welsh">Welsh</option>
              <option value="White Scottish">Scottish</option>
              <option value="White Northern Irish">Northern Irish</option>
              <option value="White Irish">Irish</option>
              <option value="White Gypsy or Irish Traveller">Gypsy or Irish Traveller</option>
              <option value="White Other">Any other White background</option>
            </optgroup>
            <optgroup label="Mixed or Multiple ethnic groups">
              <option value="Mixed White and Black Caribbean">White and Black Caribbean</option>
              <option value="Mixed White and Black African">White and Black African</option>
              <option value="Mixed White Other">Any other Mixed or Multiple background</option>
            </optgroup>
            <optgroup label="Asian">
              <option value="Asian Indian">Indian</option>
              <option value="Asian Pakistani">Pakistani</option>
              <option value="Asian Bangladeshi">Bangladeshi</option>
              <option value="Asian Chinese">Chinese</option>
              <option value="Asian Other">Any other Asian background</option>
            </optgroup>
            <optgroup label="Black">
              <option value="Black African">African</option>
              <option value="Black African American">African American</option>
              <option value="Black Caribbean">Caribbean</option>
              <option value="Black Other">Any other Black background</option>
            </optgroup>
            <optgroup label="Other ethnic groups">
              <option value="Arab">Arab</option>
              <option value="Hispanic">Hispanic</option>
              <option value="Latino">Latino</option>
              <option value="Native American">Native American</option>
              <option value="Pacific Islander">Pacific Islander</option>
              <option value="Other">Any other ethnic group</option>
            </optgroup>
          </select>
        </fieldset>
      </p>
    </div>
    `;
}

function getCountryField() {
    return `
    <div class="form-group request-country">
      <span class="request-view"></span>
      <div class="request-edit">
        <label for="Input_Country"><b>Country of residence</b></label>
        <select class="form-control" id="Input_Country" name="Input.Country">
          <option value="" selected="selected">Any</option>
          <option value="Afghanistan">Afghanistan</option>
          <option value="Albania">Albania</option>
          <option value="Algeria">Algeria</option>
          <option value="American Samoa">American Samoa</option>
          <option value="Andorra">Andorra</option>
          <option value="Angola">Angola</option>
          <option value="Anguilla">Anguilla</option>
          <option value="Antartica">Antarctica</option>
          <option value="Antigua and Barbuda">Antigua and Barbuda</option>
          <option value="Argentina">Argentina</option>
          <option value="Armenia">Armenia</option>
          <option value="Aruba">Aruba</option>
          <option value="Australia">Australia</option>
          <option value="Austria">Austria</option>
          <option value="Azerbaijan">Azerbaijan</option>
          <option value="Bahamas">Bahamas</option>
          <option value="Bahrain">Bahrain</option>
          <option value="Bangladesh">Bangladesh</option>
          <option value="Barbados">Barbados</option>
          <option value="Belarus">Belarus</option>
          <option value="Belgium">Belgium</option>
          <option value="Belize">Belize</option>
          <option value="Benin">Benin</option>
          <option value="Bermuda">Bermuda</option>
          <option value="Bhutan">Bhutan</option>
          <option value="Bolivia">Bolivia</option>
          <option value="Bosnia and Herzegowina">Bosnia and Herzegowina</option>
          <option value="Botswana">Botswana</option>
          <option value="Bouvet Island">Bouvet Island</option>
          <option value="Brazil">Brazil</option>
          <option value="British Indian Ocean Territory">British Indian Ocean Territory</option>
          <option value="Brunei Darussalam">Brunei Darussalam</option>
          <option value="Bulgaria">Bulgaria</option>
          <option value="Burkina Faso">Burkina Faso</option>
          <option value="Burundi">Burundi</option>
          <option value="Cambodia">Cambodia</option>
          <option value="Cameroon">Cameroon</option>
          <option value="Canada">Canada</option>
          <option value="Cape Verde">Cape Verde</option>
          <option value="Cayman Islands">Cayman Islands</option>
          <option value="Central African Republic">Central African Republic</option>
          <option value="Chad">Chad</option>
          <option value="Chile">Chile</option>
          <option value="China">China</option>
          <option value="Christmas Island">Christmas Island</option>
          <option value="Cocos Islands">Cocos (Keeling) Islands</option>
          <option value="Colombia">Colombia</option>
          <option value="Comoros">Comoros</option>
          <option value="Congo">Congo</option>
          <option value="Congo">Congo, the Democratic Republic of the</option>
          <option value="Cook Islands">Cook Islands</option>
          <option value="Costa Rica">Costa Rica</option>
          <option value="Cota D'Ivoire">Cote d'Ivoire</option>
          <option value="Croatia">Croatia (Hrvatska)</option>
          <option value="Cuba">Cuba</option>
          <option value="Cyprus">Cyprus</option>
          <option value="Czech Republic">Czech Republic</option>
          <option value="Denmark">Denmark</option>
          <option value="Djibouti">Djibouti</option>
          <option value="Dominica">Dominica</option>
          <option value="Dominican Republic">Dominican Republic</option>
          <option value="East Timor">East Timor</option>
          <option value="Ecuador">Ecuador</option>
          <option value="Egypt">Egypt</option>
          <option value="El Salvador">El Salvador</option>
          <option value="Equatorial Guinea">Equatorial Guinea</option>
          <option value="Eritrea">Eritrea</option>
          <option value="Estonia">Estonia</option>
          <option value="Ethiopia">Ethiopia</option>
          <option value="Falkland Islands">Falkland Islands (Malvinas)</option>
          <option value="Faroe Islands">Faroe Islands</option>
          <option value="Fiji">Fiji</option>
          <option value="Finland">Finland</option>
          <option value="France">France</option>
          <option value="France Metropolitan">France, Metropolitan</option>
          <option value="French Guiana">French Guiana</option>
          <option value="French Polynesia">French Polynesia</option>
          <option value="French Southern Territories">French Southern Territories</option>
          <option value="Gabon">Gabon</option>
          <option value="Gambia">Gambia</option>
          <option value="Georgia">Georgia</option>
          <option value="Germany">Germany</option>
          <option value="Ghana">Ghana</option>
          <option value="Gibraltar">Gibraltar</option>
          <option value="Greece">Greece</option>
          <option value="Greenland">Greenland</option>
          <option value="Grenada">Grenada</option>
          <option value="Guadeloupe">Guadeloupe</option>
          <option value="Guam">Guam</option>
          <option value="Guatemala">Guatemala</option>
          <option value="Guinea">Guinea</option>
          <option value="Guinea-Bissau">Guinea-Bissau</option>
          <option value="Guyana">Guyana</option>
          <option value="Haiti">Haiti</option>
          <option value="Heard and McDonald Islands">Heard and Mc Donald Islands</option>
          <option value="Holy See">Holy See (Vatican City State)</option>
          <option value="Honduras">Honduras</option>
          <option value="Hong Kong">Hong Kong</option>
          <option value="Hungary">Hungary</option>
          <option value="Iceland">Iceland</option>
          <option value="India">India</option>
          <option value="Indonesia">Indonesia</option>
          <option value="Iran">Iran (Islamic Republic of)</option>
          <option value="Iraq">Iraq</option>
          <option value="Ireland">Ireland</option>
          <option value="Israel">Israel</option>
          <option value="Italy">Italy</option>
          <option value="Jamaica">Jamaica</option>
          <option value="Japan">Japan</option>
          <option value="Jordan">Jordan</option>
          <option value="Kazakhstan">Kazakhstan</option>
          <option value="Kenya">Kenya</option>
          <option value="Kiribati">Kiribati</option>
          <option value="Democratic People's Republic of Korea">Korea, Democratic People's Republic of</option>
          <option value="Korea">Korea, Republic of</option>
          <option value="Kuwait">Kuwait</option>
          <option value="Kyrgyzstan">Kyrgyzstan</option>
          <option value="Lao">Lao People's Democratic Republic</option>
          <option value="Latvia">Latvia</option>
          <option value="Lebanon">Lebanon</option>
          <option value="Lesotho">Lesotho</option>
          <option value="Liberia">Liberia</option>
          <option value="Libyan Arab Jamahiriya">Libyan Arab Jamahiriya</option>
          <option value="Liechtenstein">Liechtenstein</option>
          <option value="Lithuania">Lithuania</option>
          <option value="Luxembourg">Luxembourg</option>
          <option value="Macau">Macau</option>
          <option value="Macedonia">Macedonia, The Former Yugoslav Republic of</option>
          <option value="Madagascar">Madagascar</option>
          <option value="Malawi">Malawi</option>
          <option value="Malaysia">Malaysia</option>
          <option value="Maldives">Maldives</option>
          <option value="Mali">Mali</option>
          <option value="Malta">Malta</option>
          <option value="Marshall Islands">Marshall Islands</option>
          <option value="Martinique">Martinique</option>
          <option value="Mauritania">Mauritania</option>
          <option value="Mauritius">Mauritius</option>
          <option value="Mayotte">Mayotte</option>
          <option value="Mexico">Mexico</option>
          <option value="Micronesia">Micronesia, Federated States of</option>
          <option value="Moldova">Moldova, Republic of</option>
          <option value="Monaco">Monaco</option>
          <option value="Mongolia">Mongolia</option>
          <option value="Montserrat">Montserrat</option>
          <option value="Morocco">Morocco</option>
          <option value="Mozambique">Mozambique</option>
          <option value="Myanmar">Myanmar</option>
          <option value="Namibia">Namibia</option>
          <option value="Nauru">Nauru</option>
          <option value="Nepal">Nepal</option>
          <option value="Netherlands">Netherlands</option>
          <option value="Netherlands Antilles">Netherlands Antilles</option>
          <option value="New Caledonia">New Caledonia</option>
          <option value="New Zealand">New Zealand</option>
          <option value="Nicaragua">Nicaragua</option>
          <option value="Niger">Niger</option>
          <option value="Nigeria">Nigeria</option>
          <option value="Niue">Niue</option>
          <option value="Norfolk Island">Norfolk Island</option>
          <option value="Northern Mariana Islands">Northern Mariana Islands</option>
          <option value="Norway">Norway</option>
          <option value="Oman">Oman</option>
          <option value="Pakistan">Pakistan</option>
          <option value="Palau">Palau</option>
          <option value="Panama">Panama</option>
          <option value="Papua New Guinea">Papua New Guinea</option>
          <option value="Paraguay">Paraguay</option>
          <option value="Peru">Peru</option>
          <option value="Philippines">Philippines</option>
          <option value="Pitcairn">Pitcairn</option>
          <option value="Poland">Poland</option>
          <option value="Portugal">Portugal</option>
          <option value="Puerto Rico">Puerto Rico</option>
          <option value="Qatar">Qatar</option>
          <option value="Reunion">Reunion</option>
          <option value="Romania">Romania</option>
          <option value="Russia">Russian Federation</option>
          <option value="Rwanda">Rwanda</option>
          <option value="Saint Kitts and Nevis">Saint Kitts and Nevis</option>
          <option value="Saint LUCIA">Saint LUCIA</option>
          <option value="Saint Vincent">Saint Vincent and the Grenadines</option>
          <option value="Samoa">Samoa</option>
          <option value="San Marino">San Marino</option>
          <option value="Sao Tome and Principe">Sao Tome and Principe</option>
          <option value="Saudi Arabia">Saudi Arabia</option>
          <option value="Senegal">Senegal</option>
          <option value="Seychelles">Seychelles</option>
          <option value="Sierra">Sierra Leone</option>
          <option value="Singapore">Singapore</option>
          <option value="Slovakia">Slovakia (Slovak Republic)</option>
          <option value="Slovenia">Slovenia</option>
          <option value="Solomon Islands">Solomon Islands</option>
          <option value="Somalia">Somalia</option>
          <option value="South Africa">South Africa</option>
          <option value="South Georgia">South Georgia and the South Sandwich Islands</option>
          <option value="Span">Spain</option>
          <option value="SriLanka">Sri Lanka</option>
          <option value="St. Helena">St. Helena</option>
          <option value="St. Pierre and Miguelon">St. Pierre and Miquelon</option>
          <option value="Sudan">Sudan</option>
          <option value="Suriname">Suriname</option>
          <option value="Svalbard">Svalbard and Jan Mayen Islands</option>
          <option value="Swaziland">Swaziland</option>
          <option value="Sweden">Sweden</option>
          <option value="Switzerland">Switzerland</option>
          <option value="Syria">Syrian Arab Republic</option>
          <option value="Taiwan">Taiwan, Province of China</option>
          <option value="Tajikistan">Tajikistan</option>
          <option value="Tanzania">Tanzania, United Republic of</option>
          <option value="Thailand">Thailand</option>
          <option value="Togo">Togo</option>
          <option value="Tokelau">Tokelau</option>
          <option value="Tonga">Tonga</option>
          <option value="Trinidad and Tobago">Trinidad and Tobago</option>
          <option value="Tunisia">Tunisia</option>
          <option value="Turkey">Turkey</option>
          <option value="Turkmenistan">Turkmenistan</option>
          <option value="Turks and Caicos">Turks and Caicos Islands</option>
          <option value="Tuvalu">Tuvalu</option>
          <option value="Uganda">Uganda</option>
          <option value="Ukraine">Ukraine</option>
          <option value="United Arab Emirates">United Arab Emirates</option>
          <option value="United Kingdom">United Kingdom</option>
          <option value="United States">United States</option>
          <option value="United States Minor Outlying Islands">United States Minor Outlying Islands</option>
          <option value="Uruguay">Uruguay</option>
          <option value="Uzbekistan">Uzbekistan</option>
          <option value="Vanuatu">Vanuatu</option>
          <option value="Venezuela">Venezuela</option>
          <option value="Vietnam">Viet Nam</option>
          <option value="Virgin Islands (British)">Virgin Islands (British)</option>
          <option value="Virgin Islands (U.S)">Virgin Islands (U.S.)</option>
          <option value="Wallis and Futana Islands">Wallis and Futuna Islands</option>
          <option value="Western Sahara">Western Sahara</option>
          <option value="Yemen">Yemen</option>
          <option value="Yugoslavia">Yugoslavia</option>
          <option value="Zambia">Zambia</option>
          <option value="Zimbabwe">Zimbabwe</option>
        </select>
      </div>
    </div>
    `;
}

function getAddMcButton() {
    return `
    <label><b>Medical conditions</b></label>
    <span class="request-view"></span>
    <div id="mc-list" class="mb-3">
        <div class="card mb-3" id="addition-mc-card">
          <div class="card-body">
            <p class="card-text">
            <i class="fas fa-plus"></i>
            <span id="add-mc">Click here to add</span> medical conditions.
            </p>
          </div>
        </div>
    </div>
    `;
}

function getButtons() {
    return `
    <button type="button" class="btn btn-primary request-edit" id="submit-request">
      Submit request
    </button>
    <button type="button" class="btn btn-light request-edit" id="cancel-request">
      Cancel
    </button>
    <button type="button" class="btn btn-primary request-view" id="edit-request">
      Edit
    </button>
    <button type="button" class="btn btn-danger request-view" id="revoke-request">
      Revoke
    </button>
    `;
}

function showEditMode(request) {
    request.find('.request-edit').show();
    request.find('.request-view').hide();
}

function showViewMode(request) {
  request.find('.request-view').show();
  request.find('.request-edit').hide();
}

function addNewRequestForEdit() {
    count++;
    $('#request-list').append(getRequest());
    showEditMode($('#request-list').last('.request-item'));
}

function displaySubmitted(request) {
    let description = request.find('.request-description');
    if (!!description) {
        description.removeClass('alert-warning');
        description.addClass('alert-success');
        description.html('Your request for health data from data subjects that match the following criteria has been submitted. You cannot alter your criteria. <br/>Please revoke this request and submit a new request if you would like to change your request criteria.');
    }

    let badge = request.find('.badge-warning');
    if (!!badge) {
        badge.removeClass('badge-warning');
        badge.addClass('badge-success');
        badge.html('Submitted');
    }
}

function displayDataTypeAnswer(request) {
    let dataTypeSection = request.find('.request-datatype');
    let dataTypeOptions = dataTypeSection.find('select.request-edit option');
    let selectedOptions = dataTypeOptions.filter((i, el) => el.selected);
    let dataTypeView = dataTypeSection.find('.request-view');
    let answerText = `<p>⭕ None selected</p>`;

    if (selectedOptions.length > 0) {
      answerText = '<ul>';
      selectedOptions.each((i, el) => { answerText += `<li>${el.value}</li>`})
      answerText += '</ul>'
    }

    answerText = `<b>Data type</b>${answerText}`;
    dataTypeView.html(answerText);
}

function displayGenderAnswer(request) {
    let genderSection = request.find('.request-gender');
    let genderCheckboxes = genderSection.find('input.request-edit');
    let genderView = genderSection.find('.request-view');
    let answers = genderCheckboxes.filter((i, el) => el.checked);
    let answerText = '<p>⭕ None selected</p>';

    if (answers.length > 0) {
        answerText = '<ul>';
        answers.each((i, el) => { answerText += `<li>${el.value}</li>`})
        answerText += '</ul>'
    }

    genderView.html(answerText);
}

function displayMcList(request) {
    makeMcListReadonly();
}

$('#add-purchase-request').click(addNewRequestForEdit);

$('#request-list').on('click', '#submit-request', function () {
    let request = $(this).closest('.request-item');
    
    displayMcList();
    displayDataTypeAnswer(request);
    displayGenderAnswer(request);
    showViewMode(request);
    displaySubmitted(request);
});

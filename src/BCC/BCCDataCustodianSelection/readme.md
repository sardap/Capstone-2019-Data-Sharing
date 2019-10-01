# BCC Data Custodian Selector

## Docker Info

### Building Image
`docker build --rm -t capstonegroup30/bcc-data-custodian-selector:tagname .`

### Pushing image
`docker push capstonegroup30/bcc-data-custodian-selector:tagname`

### Startup Info
Requires the following environment variables be set 
* `ValidatorIP`
* `ValidatorPort`
* `PolicyGatewayIP`
* `PolicyGatewayPort`

## Accessing
Refer to `BCC Data Custodian Selection Web Interface Gateway` found here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

### Usage

This component must be redirected to with a policy embedded in the URL, like the following example:
<url>/<policy>/<policyKey>

`https://authorization.secretwaterfall/{"excluded_categories":[0],"min_price":10,"time_period":{"start":-4785955200,"end":693705600},"data_type":"heart rate","wallet_ID":"xxxxxxxxxxxxxxxxxx","active":[false],"report_log":[{"data":"123","hash":"321"}]}/8e397dc9-695e-4e7c-b6ad-9002695618ab`

Users will enter information prompted on the web page here and select a custodian.
The web page will redireect to the appropriate custodians authorization page, where they will be prompted to login.
There will be an access token returned, and all information is accessible that was inputted by the user earlier.

## Testing component

### Test page navigation

The page or its component calls should fail if:

There is no policy/key embedded in the URL
The policy or policy key are not valid (according to the Policy Validator)
The policy or policy key cannot be placed on the Blockchain (according to the Policy Gateway)
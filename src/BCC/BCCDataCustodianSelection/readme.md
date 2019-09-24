
*Required environment variables*
ValidatorIP: IP address of the policy validator 
ValidatorPort: Port of the policy validator
PolicyGatewayIP: IP address of the policy deployer gateway
PolicyGatewayPort: Port of the policy deployer gateway

*Required policy input*
When running, a policy and a policy key is required to be passed to the page as a URL parameter.
It is required to be in the following format:
<url>/<policy>/<policykey>

It does not need to be URL encoded.

Example of policy/policyKey to add to the end of the URL:
/{"excluded_categories":[0],"min_price":10,"time_period":{"start":-4785955200,"end":693705600},"data_type":"heart rate","wallet_ID":"xxxxxxxxxxxxxxxxxx","active":[false],"report_log":[{"data":"123","hash":"321"}]}/8e397dc9-695e-4e7c-b6ad-9002695618ab

*Docker build command*
docker build --rm -t capstonegroup30/bcc-data-custodian-selector:latest .

*Docker repo to push to*
docker push capstonegroup30/bcc-data-custodian-selector:tagname


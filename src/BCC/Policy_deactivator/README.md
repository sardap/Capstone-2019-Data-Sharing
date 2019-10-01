# Policy Deactivator 

The policy deactivator is a small cli utility so that a data subject can change a policies active state to false.   

The deactivator assumes the user is running a read-only node of the chain

it's usage is:
`$deactivator <address:port> <username> <password> <wallet_id> <location> <new_data_filename.json>`

For example: `./index.js 127.0.0.1:3000 some_username some_password wallet_xyz location_foobar example_data.json`

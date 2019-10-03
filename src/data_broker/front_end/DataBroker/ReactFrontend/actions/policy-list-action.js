export const REQUEST_POLICIES = "REQUEST_POLICIES";
export const RECEIVE_POLICIES = "RECEIVE_POLICIES";

export const requestPolicies = () => ({
  type: REQUEST_POLICIES
});

export const receivePolicies = json => ({
  type: RECEIVE_POLICIES,
  policies: json.data.children.map(child => child.data) //todo
});

const fetchPolicies = () => dispatch => {
  dispatch(requestPolicies);
  return fetch(`SOME_URL`)
    .then(response => response.json())
    .then(json => dispatch(receivePolicies(json)));
};

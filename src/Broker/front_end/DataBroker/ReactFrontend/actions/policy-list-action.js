import moment from "moment";

export const REQUEST_POLICIES = "REQUEST_POLICIES";
export const RECEIVE_POLICIES = "RECEIVE_POLICIES";
export const ADD_POLICY = "ADD_POLICY";

export const addPolicyMessage = policy => ({
  type: ADD_POLICY,
  policy
});
export const addPolicy = policy => dispatch => {
  return dispatch(addPolicyMessage(policy));
};

export const requestPolicies = () => ({
  type: REQUEST_POLICIES
});

export const receivePolicies = json => ({
  type: RECEIVE_POLICIES,
  policies: json.data.map(policy => ({
    id: policy.id,
    active: policy.active,
    minPrice: policy.min_price,
    start: moment(policy.start),
    end: moment(policy.end),
    excluded: policy.excluded.split(","),
    verified: policy.verified
  }))
});

export const fetchPolicies = () => dispatch => {
  dispatch(requestPolicies());
  return fetch(`/api/GetAllPolicies`)
    .then(response => response.json())
    .then(json => dispatch(receivePolicies(json)));
};

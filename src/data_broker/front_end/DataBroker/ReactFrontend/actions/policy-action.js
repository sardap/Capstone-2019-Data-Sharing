export const ADD_NEW_POLICY = "ADD_NEW_POLICY";
export const ADD_NEW_POLICY_SUCCESS = "ADD_NEW_POLICY_SUCCESS";
export const ADD_NEW_POLICY_FAILED = "ADD_NEW_POLICY_FAILED";
export const REMOVE_POLICY = "REMOVE_POLICY";
export const ACTIVATE_POLICY = "ACTIVATE_POLICY";
export const EDIT_POLICY = "EDIT_POLICY";

export const addNewPolicyRequestMessage = () => ({
  type: ADD_NEW_POLICY
});
export const addNewPolicySuccessMessage = policy => ({
  type: ADD_NEW_POLICY_SUCCESS,
  policy: policy
});
export const addNewPolicy = policy => dispatch => {
  dispatch(addNewPolicyRequestMessage());
  console.log("ADD NEW POLICY", policy);
  return {};
  return post(`https://localhost:5001/api/addNewPolicy`);
};

export const removePolicy = id => ({
  type: REMOVE_POLICY,
  id: id
});

export const activatePolicy = id => ({
  type: ACTIVATE_POLICY,
  id: id
});

const editPolicyRequestMessage = () => ({
  type: EDIT_POLICY
});
export const editPolicy = policy => dispatch => {
  dispatch(editPolicyRequestMessage());
  return {};
  return post(`https://localhost:5001/`);
};

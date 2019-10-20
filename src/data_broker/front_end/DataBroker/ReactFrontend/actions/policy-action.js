import * as axios from "axios";
import * as HttpStatus from "http-status-codes";
import { fetchPolicies } from "./policy-list-action";

export const SAVE_NEW_POLICY = "SAVE_NEW_POLICY";
export const SAVE_NEW_POLICY_SUCCESS = "SAVE_NEW_POLICY_SUCCESS";
export const SAVE_NEW_POLICY_FAILED = "SAVE_NEW_POLICY_FAILED";
export const REMOVE_POLICY = "REMOVE_POLICY";
export const ACTIVATE_POLICY = "ACTIVATE_POLICY";
export const DEACTIVATE_POLICY = "DEACTIVATE_POLICY";
export const EDIT_POLICY = "EDIT_POLICY";
export const SHOW_TOAST = "SHOW_TOAST";

export const showToast = message => ({
  type: SHOW_TOAST,
  message
});

export const saveNewPolicyRequestMessage = () => ({
  type: SAVE_NEW_POLICY
});
export const saveNewPolicy = p => dispatch => {
  dispatch(saveNewPolicyRequestMessage());
  const policy = {
    id: "",
    min_price: p.minPrice,
    active: false,
    start: p.start.format(),
    end: p.end.format(),
    excluded: p.excluded.join(",")
  };
  return axios
    .post(`https://localhost:5001/api/AddPolicy`, policy, {
      withCredentials: true
    })
    .then(response => {
      if (response.status === HttpStatus.OK && response.data.success) {
        dispatch(saveNewPolicySuccess());
      } else {
        dispatch(showToast("Fail to add new policy 😢"));
      }
    });
};

export const saveNewPolicySuccessMessage = () => ({
  type: SAVE_NEW_POLICY_SUCCESS
});
export const saveNewPolicySuccess = () => dispatch => {
  dispatch(saveNewPolicyRequestMessage());
  return dispatch(fetchPolicies());
};

export const removePolicyMessage = id => ({
  type: REMOVE_POLICY,
  id: id
});
export const removePolicy = id => dispatch => {
  dispatch(removePolicyMessage());
  return axios
    .post(
      `https://localhost:5001/api/RemovePolicy`,
      { id },
      {
        withCredentials: true
      }
    )
    .then(response => {
      if (response.status !== HttpStatus.OK || !response.data.success) {
        dispatch(showToast("Fail to remove policy 😢"));
      } else {
        dispatch(fetchPolicies());
      }
    });
};

export const activatePolicyMessage = id => ({
  type: ACTIVATE_POLICY,
  id: id
});
export const activatePolicy = id => dispatch => {
  dispatch(activatePolicyMessage());
  return axios
    .post(
      `https://localhost:5001/api/ActivatePolicy`,
      { id },
      {
        withCredentials: true
      }
    )
    .then(response => {
      if (response.status !== HttpStatus.OK || !response.data.success) {
        dispatch(showToast("Fail to activate policy 😢"));
      } else {
        dispatch(fetchPolicies());
      }
    });
};

export const deactivatePolicyMessage = id => ({
  type: DEACTIVATE_POLICY,
  id: id
});
export const deactivatePolicy = id => dispatch => {
  dispatch(deactivatePolicyMessage());
  return axios
    .post(
      `https://localhost:5001/api/DeactivatePolicy`,
      { id },
      {
        withCredentials: true
      }
    )
    .then(response => {
      if (response.status !== HttpStatus.OK || !response.data.success) {
        dispatch(showToast("Fail to activate policy 😢"));
      } else {
        dispatch(fetchPolicies());
      }
    });
};

const editPolicyRequestMessage = () => ({
  type: EDIT_POLICY
});
export const editPolicy = p => dispatch => {
  const policy = {
    id: p.id,
    min_price: p.minPrice,
    active: false,
    start: p.start.format(),
    end: p.end.format(),
    excluded: p.excluded.join(",")
  };
  dispatch(editPolicyRequestMessage());
  return axios
    .post(`https://localhost:5001/api/UpdatePolicy`, policy, {
      withCredentials: true
    })
    .then(response => {
      if (response.status !== HttpStatus.OK || !response.data.success) {
        dispatch(showToast("Fail to update policy 😢"));
      }
    });
};

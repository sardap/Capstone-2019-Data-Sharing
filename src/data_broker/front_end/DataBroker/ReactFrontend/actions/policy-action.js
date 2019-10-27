import * as axios from "axios";
import * as HttpStatus from "http-status-codes";
import { fetchPolicies } from "./policy-list-action";
import $ from "jquery";

export const SAVE_NEW_POLICY = "SAVE_NEW_POLICY";
export const SAVE_NEW_POLICY_SUCCESS = "SAVE_NEW_POLICY_SUCCESS";
export const SAVE_NEW_POLICY_FAILED = "SAVE_NEW_POLICY_FAILED";
export const REMOVE_POLICY = "REMOVE_POLICY";
export const ACTIVATE_POLICY = "ACTIVATE_POLICY";
export const DEACTIVATE_POLICY = "DEACTIVATE_POLICY";
export const EDIT_POLICY = "EDIT_POLICY";
export const SHOW_ERROR_MODAL = "SHOW_ERROR_MODAL";
export const HIDE_ERROR_MODAL = "HIDE_ERROR_MODAL";

export const showErrorModalMessage = message => ({
  type: SHOW_ERROR_MODAL,
  message
});
export const showErrorModal = message => dispatch => {
  dispatch(showErrorModalMessage(message));
  $("#policyResultModal").modal("show");
};

export const hideErrorModalMessage = () => ({
  type: HIDE_ERROR_MODAL
});
export const hideErrorModal = message => dispatch => {
  dispatch(hideErrorModalMessage(message));
  $("#policyResultModal").modal("hide");
};

export const saveNewPolicyRequestMessage = () => ({
  type: SAVE_NEW_POLICY
});
export const saveNewPolicy = p => dispatch => {
  dispatch(saveNewPolicyRequestMessage());
  const policy = {
    id: "",
    min_price: p.minPrice,
    active: p.active,
    start: p.start.format(),
    end: p.end.format(),
    excluded: p.excluded.join(","),
    verified: false
  };

  return axios
    .post(`/api/AddPolicy`, policy, {
      withCredentials: true
    })
    .then(response => {
      if (response.status === HttpStatus.OK && response.data.success) {
        dispatch(saveNewPolicySuccess(response.data.message));
      } else {
        const sanitisedMessage = response.data.message
          .replace(/"/g, " ")
          .replace(/\[|\]/g, "");
        dispatch(saveNewPolicyFailed(sanitisedMessage));
      }
    });
};

export const saveNewPolicySuccessMessage = () => ({
  type: SAVE_NEW_POLICY_SUCCESS
});
export const saveNewPolicySuccess = link => dispatch => {
  dispatch(saveNewPolicyRequestMessage());
  dispatch(fetchPolicies());
  window.location = link;
};

export const saveNewPolicyFailedMessage = () => ({
  type: SAVE_NEW_POLICY_FAILED
});
export const saveNewPolicyFailed = message => dispatch => {
  dispatch(saveNewPolicyFailedMessage());
  dispatch(fetchPolicies());
  dispatch(showErrorModal("Fail to add new policy 😢" + message));
};

export const removePolicyMessage = id => ({
  type: REMOVE_POLICY,
  id: id
});
export const removePolicy = id => dispatch => {
  dispatch(removePolicyMessage());
  return axios
    .post(
      `/api/RemovePolicy`,
      { id },
      {
        withCredentials: true
      }
    )
    .then(response => {
      if (response.status !== HttpStatus.OK || !response.data.success) {
        dispatch(showErrorModal("Fail to remove policy 😢"));
      } else {
        dispatch(fetchPolicies());
      }
    });
};

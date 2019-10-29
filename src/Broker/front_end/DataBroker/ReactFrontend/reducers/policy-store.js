import {
  REQUEST_POLICIES,
  RECEIVE_POLICIES,
  ADD_POLICY
} from "../actions/policy-list-action";
import { SHOW_ERROR_MODAL, HIDE_ERROR_MODAL } from "../actions/policy-action";

export const policies = (
  state = {
    isFetching: false,
    policies: [],
    showErrorModal: false,
    errorModalMessage: ""
  },
  action
) => {
  switch (action.type) {
    case SHOW_ERROR_MODAL:
      return {
        ...state,
        showErrorModal: true,
        errorModalMessage: action.message
      };
    case HIDE_ERROR_MODAL:
      return {
        ...state,
        showErrorModal: false,
        errorModalMessage: ""
      };
    case ADD_POLICY:
      return {
        ...state,
        policies: [action.policy, ...state.policies],
        isFetching: false
      };
    case REQUEST_POLICIES:
      return {
        ...state,
        isFetching: true
      };
    case RECEIVE_POLICIES:
      return {
        ...state,
        isFetching: false,
        policies: action.policies
      };
    default:
      return state;
  }
};

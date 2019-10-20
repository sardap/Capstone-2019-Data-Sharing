import {
  REQUEST_POLICIES,
  RECEIVE_POLICIES,
  ADD_POLICY
} from "../actions/policy-list-action";
import { SHOW_TOAST } from "../actions/policy-action";

export const policies = (
  state = {
    isFetching: false,
    policies: [],
    showToast: false,
    toastMessage: ""
  },
  action
) => {
  switch (action.type) {
    case SHOW_TOAST:
      return {
        ...state,
        showToast: true,
        toastMessage: action.message
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

import {
  REQUEST_POLICIES,
  RECEIVE_POLICIES
} from "../actions/policy-list-action";

export const policies = (
  state = {
    isFetching: false,
    policies: []
  },
  action
) => {
  switch (action.type) {
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

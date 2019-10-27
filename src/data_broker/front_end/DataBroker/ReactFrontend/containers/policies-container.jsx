import React from "react";
import PolicyAddition from "../components/policy-addition";
import PolicyList from "../components/policy-list";
import PolicyResultModal from "../components/policy-result-modal";
import { connect } from "react-redux";
import { fetchPolicies, addPolicy } from "../actions/policy-list-action";
import { hideErrorModal } from "../actions/policy-action";
import moment from "moment";

class PoliciesContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isFetching: false,
      policies: []
    };
  }

  componentDidMount() {
    this.props.fetchPolicies();
  }

  onClickPolicyAddition() {
    const newPolicy = {
      id: "",
      active: false,
      minPrice: 0.1,
      start: moment(),
      end: moment(),
      excluded: ["none"]
    };
    this.props.addPolicy(newPolicy);
  }

  render() {
    const { isFetching, policies, errorModalMessage } = this.props;

    return (
      <>
        <PolicyResultModal
          message={errorModalMessage}
          onClose={() => {
            this.props.hideErrorModal();
          }}
        />
        <PolicyAddition
          onClick={() => {
            this.onClickPolicyAddition();
          }}
        ></PolicyAddition>
        {isFetching ? (
          <div className="d-flex justify-content-center">
            <div className="spinner-border text-primary" role="status"></div>
            <div className="ml-2">Loading your policies...</div>
          </div>
        ) : (
          <PolicyList policies={policies} />
        )}
      </>
    );
  }
}

const mapStateToProps = state => {
  const { isFetching, policies, errorModalMessage } = state;
  return { isFetching, policies, errorModalMessage };
};
const mapDispatchToProps = dispatch => {
  return {
    addPolicy: policy => dispatch(addPolicy(policy)),
    hideErrorModal: () => dispatch(hideErrorModal()),
    fetchPolicies: () => dispatch(fetchPolicies())
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(PoliciesContainer);

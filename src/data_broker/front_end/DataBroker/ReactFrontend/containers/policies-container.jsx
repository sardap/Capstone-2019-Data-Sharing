import React from "react";
import PolicyAddition from "../components/policy-addition";
import PolicyList from "../components/policy-list";
import { connect } from "react-redux";
import { fetchPolicies, addPolicy } from "../actions/policy-list-action";
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
    const { dispatch } = this.props;
    dispatch(fetchPolicies());
  }

  onClickPolicyAddition() {
    const { dispatch } = this.props;
    const newPolicy = {
      id: "",
      active: false,
      minPrice: 0.1,
      start: moment(),
      end: moment(),
      excluded: ["none"]
    };
    dispatch(addPolicy(newPolicy));
  }

  render() {
    const { isFetching, policies } = this.props;
    return (
      <>
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
  const { isFetching, policies } = state;
  return { isFetching, policies };
};

export default connect(mapStateToProps)(PoliciesContainer);

import React from "react";
import PolicyAddition from "../components/policy-addition";
import PolicyList from "../components/policy-list";
import { connect } from "react-redux";
import { fetchPolicies } from "../actions/policy-list-action";
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
    const newPolicy = {
      id: "",
      active: false,
      minPrice: 0.1,
      start: moment(),
      end: moment(),
      excluded: ["None"]
    };
    this.setState({ policies: [newPolicy, ...this.state.policies] });
  }

  render() {
    const { isFetching, policies } = this.props;
    return (
      <>
        <PolicyAddition></PolicyAddition>
        {isFetching ? (
          <div
            className="spinner-border text-primary"
            role="status"
            align="middle"
          >
            <span className="sr-only">Loading...</span>
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

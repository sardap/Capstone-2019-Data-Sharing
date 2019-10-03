import React from "react";
import PolicyAddition from "../components/policy-addition";
import PolicyList from "../components/policy-list";

class PoliciesContainer extends React.Component {
  render() {
    return (
      <>
        <PolicyAddition></PolicyAddition>
        <PolicyList />
      </>
    );
  }
}

export default PoliciesContainer;

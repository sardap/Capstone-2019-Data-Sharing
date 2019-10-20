import React from "react";
import Policy from "./policy";

class PolicyList extends React.Component {
  render() {
    const { policies } = this.props;
    return (
      <>
        {policies.map((p, i) => (
          <Policy
            mode={p.id === "" ? "EDIT" : "READ"}
            policy={p}
            key={p.id}
            index={i + 1}
          />
        ))}
      </>
    );
  }
}

export default PolicyList;

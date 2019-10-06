import React from "react";

class PolicyAddition extends React.Component {
  render() {
    return (
      <div className="card mb-3">
        <div className="card-body">
          <p className="card-text">
            <i className="fas fa-plus mr-2"></i>
            <span id="add-dsp">Click here to add</span> a new data sharing
            policy.
          </p>
        </div>
      </div>
    );
  }
}

export default PolicyAddition;

import React from "react";

class PolicyAddition extends React.Component {
  render() {
    return (
      <div class="card mb-3">
        <div class="card-body">
          <p class="card-text">
            <i class="fas fa-plus mr-2"></i>
            <span id="add-dsp">Click here to add</span> a new data sharing
            policy.
          </p>
        </div>
      </div>
    );
  }
}

export default PolicyAddition;

import React from "react";

export default class PolicyPriceInput extends React.PureComponent {
  render() {
    return this.props.mode === "EDIT" ? (
      <div className="form-group">
        <label>Minimum price for your biometric data</label>
        <div className="input-group">
          <div className="input-group-prepend dsp-edit">
            <div className="input-group-text">$</div>
          </div>
          <input
            type="number"
            step="0.01"
            value="0"
            className="form-control dsp-edit"
          />
        </div>
      </div>
    ) : (
      <>You will be paid at least ${0} for your biometric data.</>
    );
  }
}

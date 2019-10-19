import React from "react";

const DISPLAY_TWO_DECIMALS = 2;
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
            value={this.props.price}
            onChange={this.props.onChange}
            className="form-control dsp-edit"
          />
        </div>
      </div>
    ) : (
      <p>
        You will be paid at least{" "}
        <strong>
          ${Number(this.props.price).toFixed(DISPLAY_TWO_DECIMALS)}
        </strong>{" "}
        for your biometric data.
      </p>
    );
  }
}

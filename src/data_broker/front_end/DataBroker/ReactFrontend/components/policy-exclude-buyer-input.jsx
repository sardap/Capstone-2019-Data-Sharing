import React from "react";

export default class PolicyExcludeBuyerInput extends React.PureComponent {
  render() {
    return this.props.mode === "EDIT" ? (
      <div className="dsp-excluded form-group">
        <label className="dsp-edit">
          Select none or more to exclude your biometric data from buyers
        </label>
        <select multiple className="form-control dsp-edit">
          <option>Foo search</option>
          <option>Bar search</option>
          <option value="" selected>
            None
          </option>
        </select>
      </div>
    ) : (
      <div>TODO: exclude values</div>
    );
  }
}

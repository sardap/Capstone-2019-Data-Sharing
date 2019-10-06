import React from "react";

export default class PolicyExcludeBuyerInput extends React.PureComponent {
  render() {
    const { mode, excluded } = this.props;
    const excludedOptions = [
      { label: "None", key: "none" },
      { label: "Foo search", key: "foo" },
      { label: "Bar search", key: "bar" }
    ];
    const selectedOptionsLabels = excludedOptions
      .filter(v => excluded.includes(v.key))
      .map(v => v.label);

    return mode === "EDIT" ? (
      <div className="dsp-excluded form-group">
        <label className="dsp-edit">
          Select none or more to exclude your biometric data from buyers
        </label>
        <select multiple className="form-control dsp-edit">
          {excludedOptions.map(option => (
            <option
              key={option.key}
              value={option.key}
              selected={excluded.includes(option.key) ? "selected" : ""}
            >
              {option.label}
            </option>
          ))}
        </select>
      </div>
    ) : (
      <p>
        <label>
          Your data will not be shared or available for purchase to the
          following buyers
        </label>
        {selectedOptionsLabels.map(v => (
          <>
            <br />⛔ <strong>{v}</strong>
          </>
        ))}
      </p>
    );
  }
}

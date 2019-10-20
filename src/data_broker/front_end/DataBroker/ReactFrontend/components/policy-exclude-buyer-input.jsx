import React from "react";

export default class PolicyExcludeBuyerInput extends React.PureComponent {
  render() {
    const { mode, excluded, onChange } = this.props;
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
        <select
          multiple={true}
          className="form-control dsp-edit"
          onChange={onChange}
          onInput={onChange}
          value={excludedOptions
            .filter(v => excluded.includes(v.key))
            .map(v => v.key)}
        >
          {excludedOptions.map(option => (
            <option key={option.key} value={option.key}>
              {option.label}
            </option>
          ))}
        </select>
      </div>
    ) : (
      <p>
        {selectedOptionsLabels.length === 1 &&
        selectedOptionsLabels[0] === "None" ? (
          <label>
            Your data will be available for purchase to{" "}
            <strong>all buyers</strong>.
          </label>
        ) : (
          <>
            <label>
              Your data will not be shared or available for purchase to the
              following buyers:
            </label>
            {selectedOptionsLabels.map(v => (
              <>
                <br />⛔ <strong>{v}</strong>
              </>
            ))}
          </>
        )}
      </p>
    );
  }
}

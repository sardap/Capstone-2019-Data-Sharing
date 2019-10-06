import React from "react";
import DateTimeRangeSelector from "./datetime-range-selector";
import PolicyModeButtonGroup from "./policy-mode-button-group";
import PolicyToggleButton from "./policy-toggle-button";
import PolicyPriceInput from "./policy-price-input";
import PolicyExcludeBuyerInput from "./policy-exclude-buyer-input";

class Policy extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      mode: this.props.mode,
      enabled: true
    };
  }

  render() {
    const { policy, index } = this.props;
    return (
      <div className="card dsp-card mb-3" id="dsp_0">
        <div className="card-header" id="heading_0">
          <h4 className="mb-0">
            <button
              className="btn btn-link"
              type="button"
              data-toggle="collapse"
              data-target="#collapse_0"
              aria-expanded="true"
              aria-controls="collapse_0"
            >
              Data Sharing Policy {index}
            </button>
            <div className="badge badge-pill badge-success float-right mt-1">
              {policy.active ? "Active" : "Disabled"}
            </div>
          </h4>
        </div>

        <div
          id="collapse_0"
          className="collapse show"
          aria-labelledby="heading_0"
          data-parent="#dsp_0"
        >
          <div className="card-body">
            <PolicyExcludeBuyerInput
              mode={this.state.mode}
              excluded={policy.excluded}
            />

            <PolicyPriceInput mode={this.state.mode} price={policy.min_price} />

            <DateTimeRangeSelector
              label="Set a time range of your biometric data to share"
              mode={this.state.mode}
              start={policy.start}
              end={policy.end}
            />

            <PolicyToggleButton
              mode={this.state.mode}
              active={policy.active}
              onClick={() => {
                this.setState({ enabled: !this.state.enabled });
              }}
            />

            <PolicyModeButtonGroup
              mode={this.state.mode}
              onSave={() => this.setState({ mode: "READ" })}
              onEdit={() => this.setState({ mode: "EDIT" })}
            />
          </div>
        </div>
      </div>
    );
  }
}

export default Policy;

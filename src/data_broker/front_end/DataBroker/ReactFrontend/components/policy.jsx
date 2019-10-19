import React from "react";
import DateTimeRangeSelector from "./datetime-range-selector";
import PolicyModeButtonGroup from "./policy-mode-button-group";
import PolicyToggleButton from "./policy-toggle-button";
import PolicyPriceInput from "./policy-price-input";
import PolicyExcludeBuyerInput from "./policy-exclude-buyer-input";
import { connect } from "react-redux";
import { addNewPolicy } from "../actions/policy-action";

class Policy extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      mode: props.mode,
      policy: props.policy
    };
  }

  onSavePolicy() {
    this.setState({ mode: "READ" });
    if (this.props.policy.id === "") {
      this.props.addNewPolicy(this.state.policy);
    }
  }

  onPriceChange(e) {
    this.setState({
      policy: { ...this.state.policy, minPrice: e.target.value }
    });
    console.log("STATE", this.state);
    console.log("PROPS", this.props);
  }

  render() {
    const { index } = this.props;
    const { policy } = this.state;
    return (
      <div className="card dsp-card mb-3" id={"dsp_" + index}>
        <div className="card-header" id={"heading_" + index}>
          <h4 className="mb-0">
            <button
              className="btn btn-link"
              data-toggle="collapse"
              data-target={"#collapse_" + index}
              aria-expanded="true"
              aria-controls={"collapse_" + index}
            >
              Data Sharing Policy {index}
            </button>
            <div className="badge badge-pill badge-success float-right mt-1">
              {policy.active ? "Active" : "Disabled"}
            </div>
          </h4>
        </div>

        <div
          id={"collapse_" + index}
          className="collapse show"
          aria-labelledby={"heading_" + index}
          data-parent={"#dsp_" + index}
        >
          <div className="card-body">
            <PolicyExcludeBuyerInput
              mode={this.state.mode}
              excluded={policy.excluded}
            />

            <PolicyPriceInput
              mode={this.state.mode}
              price={policy.minPrice}
              onChange={e => this.onPriceChange(e)}
            />

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
              onSave={() => this.onSavePolicy()}
              onEdit={() => this.setState({ mode: "EDIT" })}
            />
          </div>
        </div>
      </div>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    addNewPolicy: policy => dispatch(addNewPolicy(policy))
  };
};

export default connect(
  null,
  mapDispatchToProps
)(Policy);

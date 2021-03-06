import React from "react";
import DateTimeRangeSelector from "./datetime-range-selector";
import PolicyModeButtonGroup from "./policy-mode-button-group";
import PolicyToggleButton from "./policy-toggle-button";
import PolicyPriceInput from "./policy-price-input";
import PolicyExcludeBuyerInput from "./policy-exclude-buyer-input";
import { connect } from "react-redux";
import { saveNewPolicy, removePolicy } from "../actions/policy-action";
import moment from "moment";
import * as _ from "lodash";

class Policy extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      mode: props.mode,
      policy: props.policy
    };
  }

  onSavePolicy() {
    if (this.props.policy.id === "") {
      this.props.saveNewPolicy(this.state.policy);
    }
    this.setState({ mode: "READ" });
  }

  onCancelPolicy() {
    this.setState({ mode: "READ" });
    this.setState({ policy: this.props.policy });
  }

  onRemovePolicy() {
    this.props.removePolicy(this.props.policy.id);
  }

  onExcludedSelectionChange(e) {
    const selectedOptions = Array.from(e.target.options)
      .filter(o => o.selected)
      .map(o => o.value);
    this.setState({
      policy: { ...this.state.policy, excluded: selectedOptions }
    });
  }

  onPriceChange(e) {
    this.setState({
      policy: { ...this.state.policy, minPrice: parseFloat(e.target.value) }
    });
  }

  onStartDateChange(e) {
    const input = moment(e.target.value);
    const newStart = _.cloneDeep(this.state.policy.start).set({
      date: input.date(),
      month: input.month(),
      year: input.year()
    });
    this.setState({
      policy: { ...this.state.policy, start: newStart }
    });
  }

  onStartTimeChange(e) {
    const input = moment(e.target.value, "HH:mm");
    const newStart = _.cloneDeep(this.state.policy.start).set({
      hour: input.hour(),
      minute: input.minute(),
      second: input.second()
    });
    this.setState({
      policy: { ...this.state.policy, start: newStart }
    });
  }

  onEndDateChange(e) {
    const input = moment(e.target.value);
    const newEnd = _.cloneDeep(this.state.policy.end).set({
      date: input.date(),
      month: input.month(),
      year: input.year()
    });
    this.setState({
      policy: { ...this.state.policy, end: newEnd }
    });
  }

  onEndTimeChange(e) {
    const input = moment(e.target.value, "HH:mm");
    const newEnd = _.cloneDeep(this.state.policy.end).set({
      hour: input.hour(),
      minute: input.minute(),
      second: input.second()
    });
    this.setState({
      policy: { ...this.state.policy, end: newEnd }
    });
  }

  activatePolicy() {
    this.setState({
      policy: { ...this.state.policy, active: true }
    });
  }

  deactivatePolicy() {
    this.setState({
      policy: { ...this.state.policy, active: false }
    });
  }

  render() {
    const { index } = this.props;
    const { policy, mode } = this.state;
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
            <div
              className={`badge badge-pill badge-${
                (policy.id === "" || policy.verified) && policy.active
                  ? "success"
                  : "danger"
              } float-right mt-1`}
            >
              {(policy.id === "" || policy.verified) && policy.active
                ? "Active"
                : "Disabled"}
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
            {!policy.verified && mode === "READ" ? (
              <div className="alert alert-danger" role="alert">
                Your data sharing policy is NOT <strong>VERIFIED</strong> and{" "}
                therefore, it's currently inactive even if you have activated it{" "}
                on creation.
              </div>
            ) : (
              ""
            )}
            <PolicyExcludeBuyerInput
              mode={this.state.mode}
              excluded={policy.excluded}
              onChange={e => {
                this.onExcludedSelectionChange(e);
              }}
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
              onStartDateChange={e => this.onStartDateChange(e)}
              onStartTimeChange={e => this.onStartTimeChange(e)}
              onEndDateChange={e => this.onEndDateChange(e)}
              onEndTimeChange={e => this.onEndTimeChange(e)}
            />

            <PolicyToggleButton
              mode={policy.id === "" ? "EDIT" : "READ"}
              active={(policy.id === "" || policy.verified) && policy.active}
              onActivate={() => {
                this.activatePolicy();
              }}
              onDeactivate={() => {
                this.deactivatePolicy();
              }}
            />

            <PolicyModeButtonGroup
              mode={this.state.mode}
              onSave={() => this.onSavePolicy()}
              onEdit={() => this.setState({ mode: "EDIT" })}
              onCancel={() => this.onCancelPolicy()}
              onRemove={() => this.onRemovePolicy()}
            />
          </div>
        </div>
      </div>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    saveNewPolicy: policy => dispatch(saveNewPolicy(policy)),
    removePolicy: id => dispatch(removePolicy(id))
  };
};

export default connect(
  null,
  mapDispatchToProps
)(Policy);

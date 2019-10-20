import React from "react";
import DateTimeRangeSelector from "./datetime-range-selector";
import PolicyModeButtonGroup from "./policy-mode-button-group";
import PolicyToggleButton from "./policy-toggle-button";
import PolicyPriceInput from "./policy-price-input";
import PolicyExcludeBuyerInput from "./policy-exclude-buyer-input";
import { connect } from "react-redux";
import {
  saveNewPolicy,
  removePolicy,
  editPolicy,
  activatePolicy,
  deactivatePolicy
} from "../actions/policy-action";
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
    } else {
      this.props.editPolicy(this.state.policy);
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
            <div
              className={`badge badge-pill badge-${
                policy.active ? "success" : "danger"
              } float-right mt-1`}
            >
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
              mode={this.state.mode}
              active={policy.active}
              disabled={this.props.policy.id === ""}
              onActivate={() => {
                this.props.activatePolicy(this.props.policy.id);
              }}
              onDeactivate={() => {
                this.props.deactivatePolicy(this.props.policy.id);
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
    editPolicy: policy => dispatch(editPolicy(policy)),
    removePolicy: id => dispatch(removePolicy(id)),
    activatePolicy: id => dispatch(activatePolicy(id)),
    deactivatePolicy: id => dispatch(deactivatePolicy(id))
  };
};

export default connect(
  null,
  mapDispatchToProps
)(Policy);

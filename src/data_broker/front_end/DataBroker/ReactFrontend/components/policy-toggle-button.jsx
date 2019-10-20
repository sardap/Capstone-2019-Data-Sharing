import React from "react";

export default class PolicyToggleButton extends React.PureComponent {
  render() {
    return this.props.mode === "EDIT" ? (
      <div className="form-group">
        <label>
          Toggle this data sharing policy's status
          <small className="form-text text-muted">
            By setting the data sharing policy status to active, the
            restrictions which you specified here will be enforced.
          </small>
        </label>
        {this.props.active ? (
          <button
            className="btn btn-block btn-danger"
            onClick={this.props.onDeactivate}
          >
            Disable data sharing policy
          </button>
        ) : (
          <button
            className="btn btn-block btn-success"
            onClick={this.props.onActivate}
          >
            Activate data sharing policy
          </button>
        )}
      </div>
    ) : (
      <p>
        Your policy is currently{" "}
        <strong>{this.props.active ? "active" : "inactive"}</strong>.{" "}
        {this.props.active
          ? "All of the conditions above are being respected by the Data Broker."
          : "The Data Broker will not enforce any of the listed conditions above."}
      </p>
    );
  }
}

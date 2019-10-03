import React from "react";

export default class PolicyToggleButton extends React.PureComponent {
  render() {
    return (
      <div className="form-group">
        <label>
          Toggle this data sharing policy's status
          <small className="form-text text-muted">
            By setting the data sharing policy status to active, the
            restrictions which you specified here will be enforced.
          </small>
        </label>
        <button
          className={
            "btn btn-block " +
            (this.props.enabled ? "btn-danger" : "btn-success")
          }
          onClick={this.props.onClick}
        >
          {this.props.enabled
            ? "Disable data sharing policy"
            : "Activate data sharing policy"}
        </button>
      </div>
    );
  }
}

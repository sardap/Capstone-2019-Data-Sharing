import React from "react";

class PolicyModeButtonGroup extends React.PureComponent {
  render() {
    return this.props.mode === "EDIT" ? (
      <>
        <button className="btn btn-primary" onClick={this.props.onSave}>
          Save
        </button>{" "}
        <button className="btn btn-light" onClick={this.props.onCancel}>
          Cancel
        </button>
      </>
    ) : (
      <>
        <button className="btn btn-info" onClick={this.props.onEdit}>
          Edit
        </button>{" "}
        <button className="btn btn-danger" onClick={this.props.onRemove}>
          Remove
        </button>
      </>
    );
  }
}

export default PolicyModeButtonGroup;

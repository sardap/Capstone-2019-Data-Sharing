import React from "react";

class PolicyModeButtonGroup extends React.PureComponent {
  render() {
    return this.props.mode === "EDIT" ? (
      <>
        <button
          type="button"
          className="btn btn-primary dsp-edit"
          id="save-dsp"
          onClick={this.props.onSave}
        >
          Save
        </button>{" "}
        <button type="button" className="btn btn-light dsp-edit">
          Cancel
        </button>
      </>
    ) : (
      <>
        <button
          type="button"
          className="btn btn-info"
          onClick={this.props.onEdit}
        >
          Edit
        </button>{" "}
        <button type="button" className="btn btn-danger">
          Remove
        </button>
      </>
    );
  }
}

export default PolicyModeButtonGroup;

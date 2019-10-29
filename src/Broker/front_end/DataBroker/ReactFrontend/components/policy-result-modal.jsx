import React from "react";

export default class PolicyResultModal extends React.PureComponent {
  render() {
    const { message } = this.props;
    return (
      <div
        className="modal fade"
        id="policyResultModal"
        tabIndex="-1"
        role="dialog"
        aria-labelledby="policyResultModalTitle"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-dialog-centered" role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="policyResultModalTitle">
                Uh-oh ...
              </h5>
              <button
                type="button"
                className="close"
                data-dismiss="modal"
                aria-label="Close"
                onClick={this.props.onClose}
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">{message}</div>
            <div className="modal-footer">
              <button
                type="button"
                className="btn btn-secondary"
                data-dismiss="modal"
                onClick={this.props.onClose}
              >
                Close
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

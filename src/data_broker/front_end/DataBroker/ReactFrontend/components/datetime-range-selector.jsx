import React from "react";
import moment from "moment";

class DateTimeRangeSelector extends React.Component {
  render() {
    return this.props.mode === "EDIT" ? (
      <div className="form-group">
        <label>{this.props.label}</label>
        <div className="row">
          <div className="col">
            <input
              type="date"
              className="form-control dsp-edit dsp-start-date"
            />
          </div>
          <div className="col">
            <input
              type="time"
              className="form-control dsp-edit dsp-start-time"
            />
          </div>

          <div className="col">
            <input type="date" className="form-control dsp-edit dsp-end-date" />
          </div>
          <div className="col">
            <input type="time" className="form-control dsp-edit dsp-end-time" />
          </div>
        </div>
      </div>
    ) : (
      <>
        Only data that are recorded between{" "}
        {moment(this.props.start).format("LLL")} to{" "}
        {moment(this.props.end).format("LLL")} will be available for purchases.
        <br />
        Data Broker will not record any health data outside this time range.
      </>
    );
  }
}

export default DateTimeRangeSelector;

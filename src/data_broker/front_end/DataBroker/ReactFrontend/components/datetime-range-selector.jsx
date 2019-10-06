import React from "react";

class DateTimeRangeSelector extends React.Component {
  render() {
    const { start, end } = this.props;
    return this.props.mode === "EDIT" ? (
      <div className="form-group">
        <label>{this.props.label}</label>
        <div className="row">
          <div className="col">
            <input
              type="date"
              className="form-control"
              value={start.format("YYYY-MM-DD")}
            />
          </div>
          <div className="col">
            <input
              type="time"
              className="form-control"
              value={start.format("HH:mm")}
            />
          </div>

          <div className="col">
            <input
              type="date"
              className="form-control"
              value={end.format("YYYY-MM-DD")}
            />
          </div>
          <div className="col">
            <input
              type="time"
              className="form-control"
              value={end.format("HH:mm")}
            />
          </div>
        </div>
      </div>
    ) : (
      <p>
        Only data that are recorded between{" "}
        <b>{this.props.start.format("lll")}</b> to{" "}
        <b>{this.props.end.format("lll")} </b>will be available for purchases.
        <br />
        Data Broker will not record any health data outside this time range.
      </p>
    );
  }
}

export default DateTimeRangeSelector;

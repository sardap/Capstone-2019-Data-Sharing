import React from "react";

class DateTimeRangeSelector extends React.Component {
  render() {
    const {
      label,
      start,
      end,
      onStartDateChange,
      onStartTimeChange,
      onEndDateChange,
      onEndTimeChange
    } = this.props;
    return this.props.mode === "EDIT" ? (
      <div className="form-group">
        <label>{label}</label>
        <div className="row">
          <div className="col">
            <input
              type="date"
              className="form-control"
              onChange={onStartDateChange}
              onInput={onStartDateChange}
              value={start.format("YYYY-MM-DD")}
            />
          </div>
          <div className="col">
            <input
              type="time"
              className="form-control"
              onChange={onStartTimeChange}
              onInput={onStartTimeChange}
              value={start.format("HH:mm")}
            />
          </div>

          <div className="col">
            <input
              type="date"
              className="form-control"
              onChange={onEndDateChange}
              onInput={onEndDateChange}
              value={end.format("YYYY-MM-DD")}
            />
          </div>
          <div className="col">
            <input
              type="time"
              className="form-control"
              onChange={onEndTimeChange}
              onInput={onEndTimeChange}
              value={end.format("HH:mm")}
            />
          </div>
        </div>
      </div>
    ) : (
      <p>
        Only data that are recorded between <b>{start.format("lll")}</b> to{" "}
        <b>{end.format("lll")} </b>will be available for purchases.
        <br />
        Data Broker will not record any health data outside this time range.
      </p>
    );
  }
}

export default DateTimeRangeSelector;

import React from "react";
import ReactDOM from "react-dom";
import PoliciesContainer from "./containers/policies-container";

const container = document.querySelector("#policies-container");
ReactDOM.render(React.createElement(PoliciesContainer), container);
module.hot.accept();

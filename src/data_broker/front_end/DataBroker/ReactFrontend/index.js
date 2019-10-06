import React from "react";
import ReactDOM from "react-dom";
import PoliciesContainer from "./containers/policies-container";
import { Provider } from "react-redux";
import thunk from "redux-thunk";
import { createStore, applyMiddleware } from "redux";
import { policies } from "./reducers/policy-store";

const container = document.querySelector("#policies-container");
const middleware = [thunk];
const store = createStore(policies, applyMiddleware(...middleware));
ReactDOM.render(
  <Provider store={store}>
    <PoliciesContainer />
  </Provider>,
  container
);
module.hot.accept();

import logoIcon from "./logo.png";
import './logo.css'
import { Component } from "react";

class Logo extends Component {
  render() {
    return (
      <a className="logo" href="/">
        <img className="logo-img" alt="ecommerce" src={logoIcon}/>
      </a>
    );
  }
}

export default Logo;

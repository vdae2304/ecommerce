import shoppingCartIcon from "./shopping-cart.png";
import './shopping-cart.css'
import { Component } from "react";

class ShoppingCart extends Component {
  constructor(props) {
    super(props);
    this.state = {
      value: 0
    };
  }

  render() {
    return (
      <a className="shopping-cart" href="/cart/">
        Mi Carrito
        <img className="shopping-cart-img" alt="" src={shoppingCartIcon}/>
        <span className="shopping-cart-counter">
          {`\u00d7${this.state.value}`}
        </span>
      </a>
    );
  }
}

export default ShoppingCart;

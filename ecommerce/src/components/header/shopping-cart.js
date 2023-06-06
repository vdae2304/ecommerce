import shoppingCartIcon from "./shopping-cart.png";
import './shopping-cart.css'
import { Component } from "react";

class ShoppingCart extends Component {
  render() {
    return (
      <a className="shopping-cart" href="/cart/">
        Mi Carrito
        <img className="shopping-cart-img" src={shoppingCartIcon}/>
        <span className="shopping-cart-counter">
          {`\u00d7${this.props.value ?? 0}`}
        </span>
      </a>
    );
  }
}

export default ShoppingCart;

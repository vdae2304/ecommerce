import shoppingCartIcon from "./header/shopping-cart.png";
import './add-to-cart.css'
import { Component } from "react";

class AddShoppingCart extends Component {
  render() {
    return (
      <button className="primary-button">
        Agregar al carrito
        <img className="icon" src={shoppingCartIcon} />
      </button>
    );
  }
}

export default AddShoppingCart;

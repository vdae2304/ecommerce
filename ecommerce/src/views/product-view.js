import './product-view.css';
import { Component } from 'react';
import { useParams } from 'react-router-dom';

import Logo from '../components/header/logo';
import SearchBar from '../components/header/search-bar';
import ShoppingCart from '../components/header/shopping-cart';

import AddShoppingCart from '../components/add-to-cart';

class ProductView extends Component {
  constructor(props) {
    super(props);
    this.state = {
      sku: "",
      name: "",
      price: "",
      category: "",
      brand: "",
      image: "",
      available: true,
      stock: ""
    };
  }

  componentDidMount() {
    fetch(`/api/v1/products/${this.props.params.id}`)
      .then((response) => response.json())
      .then((data) => data[0])
      .then((data) => this.setState(data))
      .catch((error) => {
        console.log(error);
        window.location.replace("/");
      });
  }

  render() {
    return (
      <div>
        <div className="header-container">
          <Logo/>
          <SearchBar/>
          <ShoppingCart/>
        </div>
        <div className="product-contentainer">
          <div className="product-image-container">
            <img className="product-image"
                 alt={this.state.name}
                 src={this.state.image}/>
          </div>
          <div class="product-info-container">
            <h1 className="product-title">{this.state.name}</h1>
            <p className="product-metadata-container">
              <span className="product-metadata">SKU: </span>
              <span className="product-metadata-value">{this.state.sku}</span>
            </p>
            <p className="product-metadata-value">
              {this.state.available && this.state.stock > 0
                ? "Disponible"
                : "No disponible"}
            </p>
            <div className="product-add-cart-container">
              <p className="product-price">${this.state.price}</p>
              <label className="product-quantity-label">
                Cantidad:
                <input className="product-quantity"
                       type="number"
                       min="1"
                       max={Math.min(this.state.stock, 10)}
                       step="1"/>
              </label>
              <AddShoppingCart/>
            </div>
            <p className="product-metadata-container">
              <span className="product-metadata">Categoría: </span>
              <span className="product-metadata-value">
                {this.state.category}
              </span>
            </p>
            <p className="product-metadata-container">
              <span className="product-metadata">Marca: </span>
              <span className="product-metadata-value">
                {this.state.brand}
              </span>
            </p>
          </div>
        </div>
      </div>
    );
  }
};

export default () => <ProductView params={useParams()} />;

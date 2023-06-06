import './product-card.css';
import { Component } from 'react';

class ProductCard extends Component {
  render() {
    return (
      <div className="product-card-container">
        <a href={this.props.url} className="product-card-image-container">
          <img alt={this.props.name}
               className="product-card-image"
               src={this.props.image} />
        </a>
        <div className="product-card-info-container">
          <a className="product-card-title" href={this.props.url}>
            {this.props.name}
          </a>
          <p className="product-card-price">${this.props.price}</p>
          <p className="product-card-metadata-container">
            <span className="product-card-metadata">Categoría: </span>
            <span className="product-card-metadata-value">
              {this.props.category}
            </span>
          </p>
          <p className="product-card-metadata-container">
            <span className="product-card-metadata">Marca: </span>
            <span className="product-card-metadata-value">
              {this.props.brand}
            </span>
          </p>
        </div>
      </div>
    );
  }
};

export default ProductCard;

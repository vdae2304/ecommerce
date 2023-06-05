import '../App.css';
import { Component } from 'react';

export class Product extends Component {
  render() {
    return (
      <div className='product-container'>
        <a href={this.props.url} className='product-image-container'>
          <img alt={this.props.name}
            className='product-image'
            src={this.props.image} />
        </a>
        <div className='product-info-container'>
          <a className='product-title' href={this.props.url}>
            {this.props.name}
          </a>
          <p className='product-price'>${this.props.price}</p>
          <p className='product-metadata'>Categoría: </p>
          <p className='product-metadata-value'>{this.props.category}</p>
          <p className='product-metadata'>Marca: </p>
          <p className='product-metadata-value'>{this.props.brand}</p>
        </div>
      </div>
    );
  }
};

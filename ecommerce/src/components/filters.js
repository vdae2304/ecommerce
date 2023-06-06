import './filters.css';
import { Component } from 'react';

class Filters extends Component {
  render() {
    return (
      <form className="filters-container"
            onSubmit={this.props.onSubmitHandler}>
        <label className="filters-label">Categoría:</label>
        {this.props.categories.map(
          (category, index) => <label className="filters-category" key={index}>
              <input className="filters-category-option"
                     name="category"
                     type="radio"
                     value={category.name}
                     onChange={this.props.onChangeHandler} />
              {category.name}
            </label>
        )}
        <label className="filters-label">Marca:</label>
        <input className="filters-brand"
               name="brand"
               list="brands"
               type="text"
               onChange={this.props.onChangeHandler} />
        <datalist id="brands">
          {this.props.brands.map(
            (brand, index) => <option key={index} value={brand.name} />
          )}
        </datalist>
        <label className="filters-label">Precio:</label>
        <div className="filters-price-container">
          <input className="filters-price"
                 name="minPrice"
                 placeholder="$ Mínimo"
                 type="number"
                 min="0"
                 step="0.01"
                 onChange={this.props.onChangeHandler} />
          <input className="filters-price"
                 name="maxPrice"
                 placeholder="$ Máximo"
                 type="number"
                 min="0"
                 step="0.01"
                 onChange={this.props.onChangeHandler} />
        </div>
        <label className="filters-label">Ordenar por:</label>
        <select className="filters-sort-by"
                name="sortBy"
                onChange={this.props.onChangeHandler}>
          <option key="default" value="">
            Relevancia
          </option>
          <option key="name-asc" value="name-asc">
            Nombre: A-Z
          </option>
          <option key="name-desc" value="name-desc">
            Nombre: Z-A
          </option>
          <option key="price-asc" value="price-asc">
            Precio: de menor a mayor
          </option>
          <option key="price-desc" value="price-desc">
            Precio: de mayor a menor
          </option>
        </select>
        <input className="filters-submit"
               type="submit"
               value="Aplicar filtros" />
      </form>
    );
  }
};

export default Filters;

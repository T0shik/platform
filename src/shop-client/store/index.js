export const state = () => ({
  open: false,
  products: [],
  cart: null
})

export const mutations = {
  putCart(state, cart) {

  },
  putProducts(state, products) {
    console.log(products)
    state.products = products
  },
}
export const actions = {
  async nuxtServerInit({dispatch}) {
    await dispatch('loadProducts')
  },
  loadCart({commit}) {
    this.$axios
  },
  loadProducts({commit}) {
    return this.$axios.$get('/api/products')
      .then(products => commit('putProducts', products))
  }
}

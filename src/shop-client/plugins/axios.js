export default function ({$axios}) {
  $axios.onRequest(config => {
    config.withCredentials = true
  })
}

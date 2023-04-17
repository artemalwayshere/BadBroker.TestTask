<template>
  <div class="container">
    <TradeCard></TradeCard>
  </div>
</template>

<script>
import TradeCard from './components/TradeCard.vue'

export default {
  name: 'App',
  components: {
    TradeCard
  }
}
</script>

<style>
.container{
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
}
</style>

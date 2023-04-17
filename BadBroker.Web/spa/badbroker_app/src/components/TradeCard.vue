<script setup>
import Toast from "primevue/toast";
import { useToast } from "primevue/usetoast";
const toast = useToast();

const showError = () => {
  toast.add({
    severity: "info",
    summary: "Info Message",
    detail: "Here could be your toast",
    life: 3000,
  });
};
</script>

<template>
  <Toast></Toast>
  <div class="Trade">
    <form @submit="calculate" method="get">
      <h1>Select period</h1>
      <h3>Choose a period up to 60 days</h3>
      <div class="dates">
        <span class="p-float-label">
          <CalendarPicker
            class="date"
            v-model="rateQuery.startDate"
            showIcon
            inputId="start_date"
          />
          <label for="start_date">Start date</label>
        </span>
        <span class="p-float-label">
          <CalendarPicker
            v-model="rateQuery.endDate"
            showIcon
            inputId="end_date"
          />
          <label for="end_date">End date</label>
        </span>
        <span class="p-float-label">
          <InputNumber
            class="usd"
            v-model="rateQuery.moneyUsd"
            inputId="integeronly"
          />
          <label for="integeronly">Type usd amount</label>
        </span>
      </div>
      <div class="btn-sbmt">
        <ButtonSubmit label="Calculate" type="submit" @click="showError"/>
      </div>
    </form>
    <h1>The best trade</h1>
    <table class="table-result">
      <tbody>
        <tr>
          <th class="head">Buy date</th>
          <th class="head">Sell date</th>
          <th class="head">Revenue</th>
          <th class="head">Currancy</th>
        </tr>
        <tr>
          <td class="cell">{{ dateTime(tradeResult[0].buyDate) }}</td>
          <td class="cell">{{ dateTime(tradeResult[0].sellDate) }}</td>
          <td class="cell">{{ tradeResult[0].revenue }}</td>
          <td class="cell">{{ tradeResult[0].currency }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import axios from "axios";
import moment from "moment";

export default {
  name: "TradeCard",
  components: {},

  data() {
    return {
      rateQuery: {
        startDate: null,
        endDate: null,
        moneyUsd: null,
      },
      tradeResult: [{ currency: "", buyDate: "", sellDate: "", revenue: 0 }],
    };
  },

  methods: {
    async calculate(e) {
      e.preventDefault();
      const item = this.tradeResult;
      var config = {
        method: "post",
        url: "https://localhost:44333/trade/getBestTrade",
        headers: {
          "Content-Type": "application/json",
        },
        data: JSON.stringify(this.rateQuery),
      };

      await axios(config)
        .then((response) => {
          console.log(this.tradeResult);
          item[0] = response.data;
        })
        .catch(function (error) {
          console.log(error);
        });
    },

    dateTime(value) {
      if (value == "") return value;
      else return moment(value).format("YYYY-MM-DD");
    },
  },
};
</script>

<style>
.Trade {
  width: 800px;
  height: 400px;
  background-color: white;
  box-shadow: 0 14px 28px rgba(0, 0, 0, 0.25), 0 10px 10px rgba(0, 0, 0, 0.22);
  text-align: center;
}
.dates {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 25px;
}
.date {
  margin-right: 10px;
}
.usd {
  margin-left: 10px;
}
.data-table {
  margin: auto;
  width: 700px;
}

.table-result {
  width: 80%;
  border-collapse: collapse;
  border-spacing: 0;
  margin: 0 auto;
}
.table-result,
.cell,
.head {
  border: 1px solid #595959;
}
.cell,
.head {
  padding: 3px;
  width: 30px;
  height: 35px;
}
</style>

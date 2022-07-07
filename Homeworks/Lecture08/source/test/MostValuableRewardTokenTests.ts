import { expect } from "chai";
import { ethers } from "hardhat";

import { MostValuableRewardToken } from "../typechain";
import { constants } from "../constants";

describe("MostValuableRewardToken", function () {

    let mvrt: MostValuableRewardToken;
    
    
    beforeEach(async () => {
        const MostValuableRewardToken = await ethers.getContractFactory("MostValuableRewardToken");
        mvrt = await MostValuableRewardToken.deploy();
        await mvrt.deployed();
    })
  
    it("Should return the proper token name and symbol", async function () {
      expect(await mvrt.symbol()).to.equal(constants.MostValuableRewardToken.TOKEN_SYMBOL);
      expect(await mvrt.name()).to.equal(constants.MostValuableRewardToken.TOKEN_NAME);
    });
  
    it("Owner should mint and transfer new tokens to provided address. ", async function () {

      const [owner, addr1] = await ethers.getSigners();
      const newTokens = 100;

      await mvrt.mint(newTokens, addr1.address);
      expect(await mvrt.totalSupply()).to.equal(newTokens);
      expect(await mvrt.balanceOf(addr1.address)).to.equal(newTokens);
    });

});
import {
  Box,
  Button,
  Center,
  Flex,
  Heading,
  Stat,
  Stack,
  StatLabel,
  StatNumber,
  Text,
  Container,
  SimpleGrid,
  NumberInput,
  NumberInputField,
  NumberInputStepper,
  NumberIncrementStepper,
  NumberDecrementStepper,
} from '@chakra-ui/react';
import { ethers, utils } from 'ethers';
import { useState, useEffect } from 'react';
import abi from '../abi/abi.json';

const contractAddress = '0xEf33F157Ac43E261aa11ecc33A130d65c65656c4';

export default function Home() {
  const [signer, setSigner] = useState(null);
  const [address, setAddress] = useState('');
  const [balance, setBalance] = useState(0);
  const [contract, setContract] = useState(null);
  const [price, setPrice] = useState(0);
  const [totalSupply, setTotalSupply] = useState(0);
  const [maxSupply, setMaxSupply] = useState(0);
  const [mintQuantity, setMintQuantity] = useState(1);

  useEffect(() => {
    const initContract = async () => {
      await getContract(signer);
    };
    initContract();
  }, [signer]);

  const handleConnect = async () => {
    console.log("handle connect start")
    if (typeof window.ethereum !== 'undefined') {
      try {
        const newProvider = new ethers.providers.Web3Provider(window.ethereum);
        await newProvider.send('eth_requestAccounts', []);
        const accounts = await newProvider.listAccounts();
        const accBalance = await newProvider.getBalance(accounts[0]);
        setSigner(newProvider.getSigner());
        setAddress(accounts[0]);
        setBalance(utils.formatEther(accBalance));

        await getContract();
      } catch (error) {
        setSigner(null);
        console.error(error);
      }
    } else {
      console.error('install MetaMask');
    }
  };

  const getContract = async (signer) => {
    if (signer) {
      const nftContract = new ethers.Contract(contractAddress, abi, signer);
      const nftPrice = await nftContract.MINT_PRICE();
      const nftTotalSupply = await nftContract.totalSupply();
      const nftMaxSupply = await nftContract.MAX_SUPPLY();
      setContract(nftContract);
      setPrice(nftPrice);
      setTotalSupply(nftTotalSupply.toNumber());
      setMaxSupply(nftMaxSupply.toNumber());
    }
  };

  const handleMint = async () => {
    try {
      const options = { value: price * mintQuantity };
      await contract.mint(mintQuantity, options);

      console.log(mintQuantity);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Stack>
      <Flex
        justifyContent="space-between"
        alignItems="center"
        backgroundColor="teal"
        p={2}
      >
        <Heading as="h1" color="white">
          DApp
        </Heading>
        {address && balance ? (
          <Box alignItems="right" color="white">
            <Text textAlign="right">{address}</Text>
            <Text textAlign="right">{`${balance} ETH`}</Text>
          </Box>
        ) : (
          <Button onClick={handleConnect} colorScheme="gray">
            Connect
          </Button>
        )}
      </Flex>
      <Container centerContent maxW="100%">
        {contract && (
          <Stack>
            <Center>
              <Heading as="h2" size="md">
                Contract info
              </Heading>
            </Center>
            <SimpleGrid columns={2} spacing={5}>
              <Stat border="1px" borderRadius={5} p={1}>
                <StatLabel>Price</StatLabel>
                <StatNumber>{`${utils.formatEther(price)} ETH`}</StatNumber>
              </Stat>
              <Stat border="1px" borderRadius={5} p={1}>
                <StatLabel>NFTs minted</StatLabel>
                <StatNumber>{`${totalSupply} / ${maxSupply}`}</StatNumber>
              </Stat>
            </SimpleGrid>
            <Heading as="h2" size="md">
                Mint
              </Heading>
            <Stat border="1px" borderRadius={5} p={1}>
                <StatLabel>Quantity</StatLabel>
                <StatNumber>
                  <NumberInput
                    onChange={(valueString) => setMintQuantity(parseInt(valueString))}
                    defaultValue={mintQuantity}
                    min={1}
                    max={maxSupply - totalSupply}>
                    <NumberInputField />
                    <NumberInputStepper>
                      <NumberIncrementStepper />
                      <NumberDecrementStepper />
                    </NumberInputStepper>
                </NumberInput>
                </StatNumber>
              </Stat>
            
            <Button colorScheme="teal" onClick={handleMint}>
              Mint
            </Button>
          </Stack>
        )}
      </Container>
    </Stack>
  );
}
